using System.ComponentModel.DataAnnotations;

namespace UCABPagaloTodoWeb.Models.Request
{
    public class ConsultarPagosRequest
    {
        public string Consumidor { get; set; } = string.Empty;

        public string Servicio { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string fechaInicio { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string fechaFin { get; set; } = string.Empty;
    }
}
