
namespace UCABPagaloTodoWeb.Models.Request
{
    public class FormatosRequest
    {
        public List<string> Campos { get; set; } = new List<string>();

        public List<int> Longitud { get; set; } = new List<int>();

        public string Servicio { get; set; } = string.Empty;

        public bool Requerido { get; set; }

    }
}
