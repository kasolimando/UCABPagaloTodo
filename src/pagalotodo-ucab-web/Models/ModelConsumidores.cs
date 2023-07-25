using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Models
{
    public class ModelConsumidores
    {
        public ConsumidoresResponse ConsumidorSeleccionado { get; set; }

        public List<ConsumidoresResponse> Consumidores { get; set; }

        public bool Status { get; set; }

        public string Username { get; set; }
    }
}
