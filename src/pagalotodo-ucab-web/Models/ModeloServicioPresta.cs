using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModeloServicioPresta
    {
        public List<ServiciosResponse> Servicios { get; set; }

        public List<FormatosResponse> Formatos { get; set; } = new List<FormatosResponse> { new(), new(), new() };

        public ServiciosResponse ServicioResponseSeleccionado { get; set; }

        public List<FormatosResponse> FormatosSeleccionado { get; set; }

    }
}
