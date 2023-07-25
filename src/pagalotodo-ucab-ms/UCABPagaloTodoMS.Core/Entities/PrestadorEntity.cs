using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class PrestadorEntity : UsuarioEntity
    {
        public List<ServicioEntity>? Servicios { get; set; }


    }
}
