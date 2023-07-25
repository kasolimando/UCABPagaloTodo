
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class ConsultarPagosRequest
    {
        public string Consumidor { get; set; } = string.Empty;

        public string Servicio { get; set; } = string.Empty;

        public string fechaInicio { get; set; } = string.Empty;

        public string fechaFin { get; set; } = string.Empty;
    }
}
