using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models.Request;

namespace UCABPagaloTodoWeb.Models
{
    public class ModelosServicios
    {
        public List<ServiciosResponse> ServicioResponse { get; set; }

        public List<FormatosResponse> Formatos { get; set; } = new List<FormatosResponse> {new(), new(), new() };

        public ServicioRequest ServiciosRequest { get; set; }

        public ServiciosResponse ServicioResponseSeleccionado { get; set; }
    }
}
