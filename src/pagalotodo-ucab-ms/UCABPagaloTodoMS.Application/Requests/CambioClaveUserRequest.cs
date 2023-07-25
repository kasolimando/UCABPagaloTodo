
using Swashbuckle.AspNetCore.Annotations;

namespace UCABPagaloTodoMS.Application.Requests
{
    //Datos
    public class CambioClaveUserRequest
    { 
        public string Clave_actual { get; set; } = string.Empty;

        public string Clave_nueva { get; set; } = string.Empty;

    }
}
