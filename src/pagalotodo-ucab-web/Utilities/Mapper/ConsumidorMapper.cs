using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Utilities.Mapper
{
    public static class ConsumidorMapper
    {
        public static ConsumidorRequest MapResponseARequest(List<ConsumidoresResponse> response)
        {
            var nombre = response[0].Nombre.Split(" ");
            var docIdentidad = response[0].DocIdentidad.Split("-");
            var resquest = new ConsumidorRequest()
            {
                Correo = response[0].Correo,
                Nombre = nombre[0],
                Apellido = nombre[1],
                Direccion = response[0].Direccion,
                TipoVj = docIdentidad[0],
                DocIdentidad = docIdentidad[1]
            };
            return resquest;
        }

        public static CambioClaveUserRequest MapRequestACambioClave(ConsumidorRequest request)
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
