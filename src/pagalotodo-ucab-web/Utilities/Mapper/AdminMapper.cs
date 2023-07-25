using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Utilities.Mapper
{
    public static class AdminMapper
    {
        public static AdminsRequest MapResponseARequest(AdminsResponse response)
        {
            var nombre = response.Nombre.Split(" ");
            var docIdentidad = response.DocIdentidad.Split("-");
            var resquest = new AdminsRequest()
            {
                Correo = response.Correo,
                Nombre = nombre[0],
                Apellido = nombre[1],
                Direccion = response.Direccion,
                TipoVj = docIdentidad[0],
                DocIdentidad = docIdentidad[1]
            };
            return resquest;
        }

        public static CambioClaveUserRequest MapRequestACambioClave(AdminsRequest request)
        {
            var resquest = new CambioClaveUserRequest()
            {
                Clave_actual = request.Clave,
                Clave_nueva = request.Nombre
            };
            return resquest;
        }
    }
}
