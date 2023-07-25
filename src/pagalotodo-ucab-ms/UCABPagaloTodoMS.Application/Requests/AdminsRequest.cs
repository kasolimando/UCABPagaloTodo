using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class AdminsRequest
    {
        public string Clave { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string DocIdentidad { get; set; } = string.Empty;

        public string TipoVj { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;
    }
}
