using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class ConsumidorRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string DocIdentidad { get; set; } = string.Empty;

        public string TipoVj { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public bool Estatus { get; set; }     // si es 1 esta activo si es 0 esta inactivo
    }
}
