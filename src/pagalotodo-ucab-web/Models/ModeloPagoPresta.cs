using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModeloPagoPresta
    {
        public List<PagoResponse> Pagos { get; set; } = new List<PagoResponse>();

        public List<ServiciosResponse> Servicios { get; set; }

        public ConsultarPagosRequest Consulta { get; set; }

        public IFormFile file { get; set; }
    }
}
