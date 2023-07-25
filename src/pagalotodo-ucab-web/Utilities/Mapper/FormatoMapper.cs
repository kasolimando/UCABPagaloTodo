using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Utilities.Mapper
{
    public static class FormatoMapper
    {
        public static List<FormatosRequest> MapUpdateListResponseToRequest(List<FormatosResponse> formatos)
        {
            var request = new List<FormatosRequest>();
            foreach (var formato in formatos)
            {
                var singleFormato = new FormatosRequest(); // Crear un nuevo objeto para cada iteración
                singleFormato.Campos.Add(formato.Campos);
                singleFormato.Longitud.Add(formato.Longitud);
                singleFormato.Servicio = formato.Servicio;
                singleFormato.Requerido = formato.Requerido;
                request.Add(singleFormato); // Agregar el objeto a la lista
            }
            return request;
        }


        public static FormatosRequest MapListResponseToRequest(List<FormatosResponse> formatos)
        {
            var request = new FormatosRequest(); // Crear un nuevo objeto
            foreach (var formato in formatos)
            {
                request.Campos.Add(formato.Campos);
                request.Longitud.Add(formato.Longitud);
            }
            request.Servicio = formatos[0].Servicio;
            request.Requerido = true;
            return request;
        }
    }
}
