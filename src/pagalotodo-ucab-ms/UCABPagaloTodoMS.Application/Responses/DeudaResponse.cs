
namespace UCABPagaloTodoMS.Application.Responses
{
    public class DeudaResponse
    {
        public string Consumidor { get; set; } = string.Empty;

        public double Monto { get; set; }

        public string Estatus { get; set; } = string.Empty;

        public string Servicio { get; set; } = string.Empty;
    }
}
