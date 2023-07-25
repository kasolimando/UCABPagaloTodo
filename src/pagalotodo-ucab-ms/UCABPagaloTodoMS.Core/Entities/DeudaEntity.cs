

using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class DeudaEntity
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Username { get; set; } = string.Empty;

        public double Monto { get; set; }

        public Status Estatus { get; set; }

        [System.ComponentModel.DataAnnotations.Key]
        public ServicioEntity Servicio { get; set; }

        public enum Status { Activo, Inactivo }

        public Guid servicioId { get; set; }
    }
}
