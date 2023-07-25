using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Consumers
{
    public class ConsumerConciliacion : ConsumerBase, IHostedService
    {
        protected override string QueueName => "conciliacion";

        public ConsumerConciliacion(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<ConsumerConciliacion> logger,
            ILogger<ConsumerBase> baseLogger,
            ILogger<RabbitMqClientBase> clientLogger) :
            base(mediator, connectionFactory, baseLogger, clientLogger)
        {
            try
            {
                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var request = JsonConvert.DeserializeObject<GuardarConciliacionRequest>(message);
                    var tarea = await mediator.Send(new GuardarConciliacionCommand(request));
                    Console.WriteLine($"Conciliacion message received: {message}");
                };

                //read the message
                Channel.BasicConsume(queue: "conciliacion", autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}