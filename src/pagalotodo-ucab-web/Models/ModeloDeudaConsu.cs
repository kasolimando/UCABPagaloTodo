using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModeloDeudaConsu
    {
        public string Servicio { get; set; } = string.Empty;

        public List<DeudaResponse>  Deudas { get; set; }
    }
}
