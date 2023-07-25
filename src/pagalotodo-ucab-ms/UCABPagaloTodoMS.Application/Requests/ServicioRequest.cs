using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class ServicioRequest
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }  // Si la queremos que no sea null deberiamos quitarle el ?
        [Required]
        public string Estatus { get; set; } = string.Empty;

        public string? Categoria { get; set; }

        [Required]
        public string TipoPago { get; set; } = string.Empty;

        public string PrestadorEntityId { get; set; } = string.Empty;

    }
}
