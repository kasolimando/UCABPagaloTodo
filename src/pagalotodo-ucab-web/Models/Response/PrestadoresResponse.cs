namespace UCABPagaloTodoWeb.Models.Response
{
    public class PrestadoresResponse
    {
        public string Username { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string DocIdentidad { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public bool Estatus { get; set; }
    }
}
