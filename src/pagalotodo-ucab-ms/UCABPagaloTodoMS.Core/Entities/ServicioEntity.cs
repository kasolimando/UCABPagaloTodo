using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class ServicioEntity
    {

        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }  // Si la queremos que no sea null deberiamos quitarle el ?

        [Required]
        public Status Estatus { get; set; }

        public string? Categoria { get; set; }

        [Required]
        public string TipoPago { get; set; } = string.Empty;

        public string PrestadorEntityId { get; set; } = string.Empty;

        public Guid? FormatoConEntityId { get; set; }

        public FormatoConEntity Formato { get; set; }

        public List<DeudaEntity>? Deuda { get; set; }

        public PrestadorEntity Prestador { get; set; }

        public enum Status { proximamente, activo, inactivo }
    }
}
