
using System.Diagnostics.CodeAnalysis;


namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class GuardarConciliacionRequest
    {
        public string Servicio { get; set; } = string.Empty;

        public string PagoId { get; set; }

        public string Aceptado { get; set; } = string.Empty;
    }
}
