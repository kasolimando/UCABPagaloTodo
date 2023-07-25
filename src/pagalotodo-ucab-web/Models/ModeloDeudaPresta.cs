using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModeloDeudaPresta
    {
        public List<ServiciosResponse> Servicios { get; set; }

        public List<DeudaResponse> Deudas { get; set; }

        public string ServicioSeleccionado { get; set; }

        public IFormFile file { get; set; }
    }
}
