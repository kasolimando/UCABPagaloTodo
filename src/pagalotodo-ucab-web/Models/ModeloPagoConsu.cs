using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModeloPagoConsu
    {
        public List<PagoResponse> Pagos { get; set; } = new List<PagoResponse>();

        public ConsultarPagosRequest Consulta { get; set; }
    }
}
