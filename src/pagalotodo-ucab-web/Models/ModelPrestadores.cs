using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModelPrestadores
    {
        public PrestadoresResponse PrestadorSeleccionado { get; set; }

        public List<PrestadoresResponse> Prestadores { get; set; }

        public PrestadorRequest PrestadorRequest { get; set; }

        public bool Status { get; set; }

        public string Username { get; set; }
    }
}
