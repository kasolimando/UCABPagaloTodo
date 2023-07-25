using FirebaseAdmin;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UCABPagaloTodoMS.Infrastructure.Services.Firebase;

namespace UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer
{
    [ExcludeFromCodeCoverage]
    public class RabbitProducer : IRabbitProducer
    {

        // Aqui especificamos el servidor de RabbitMQ.
        public async Task SendProductMessage(IFormFile file)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            // Se crea la conexion con Rabbit
            var connection = factory.CreateConnection();

            // Se crea el canal con la sesion y el modelo
            using var channel = connection.CreateModel();

            // Se declara la cola despues del nombre y unas propiedades
            channel.QueueDeclare("deudas", exclusive: false);

            //Se llama a la funcion para descargar el contenido de firebase
            var reader = new Firebase.Firebase();
            var fileName = await reader.UploadFile(file);
            var fileContents = await reader.ReadFileContentsAsync(fileName);
            reader.FirebaseDelete();

            //Se separa por linea el archivo
            var lines = fileContents.Split('\n');


            foreach (var line in lines)
            {
                // Se serializa el mensaje y se le quitan los saltos de linea
                var palabra = line.Replace("\r", "");
                var values = palabra.Split(' ');
                var obj = new
                {
                    Servicio = values[0],
                    Username = values[1],
                    Monto = values[2]
                };
                var messageJson = JsonConvert.SerializeObject(obj);
                var messageBytes = Encoding.UTF8.GetBytes(messageJson);
                // Se ponen los datos dentro de la cola de producto
                channel.BasicPublish(exchange: "", routingKey: "deudas", basicProperties: null, body: messageBytes);
            }
        }
    }
}
