using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class UsuarioEntity
    {
        [Key]
        [StringLength(10)]
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Clave { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string DocIdentidad { get; set; } = string.Empty;

        [Required]
        public string TipoVj { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        [StringLength(25)]
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(25)]
        [Required]
        public string Apellido { get; set; } = string.Empty;

        public bool Estatus { get; set; }     // si es true esta activo si es false esta inactivo

        public string? TokenSeg { get; set; }
    }
}
