using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class StatusUserRequest
    {
        public bool Estatus { get; set; }     // si es 1 esta activo si es 0 esta inactivo
    }
}
