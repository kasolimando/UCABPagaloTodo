using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models.Request;

namespace UCABPagaloTodoWeb.Models
{
    public class ModelosServiciosConsu
    {
        public List<ServiciosResponse> ServicioResponse { get; set; }

        public PagoRequest Pago { get; set; }

        public string ServicioSeleccionado { get; set; }

        public ServicioRequest ServiciosRequest { get; set; }

        public List<DeudaResponse> Deudas { get; set; }
    }
}
