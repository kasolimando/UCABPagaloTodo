using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class StatusServicioRequest
    { 
        public string Estatus { get; set; } = string.Empty;     // si es 1 esta activo si es 0 esta inactivo


    }
}
