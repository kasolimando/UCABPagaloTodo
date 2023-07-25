using System.Net;

namespace UCABPagaloTodoMS.Application.Responses
{
    //Response genérico donde se lleva a cabo la serialización de las peticiones.
    //<T>: Parámetro genérico que será definido dependiendo del tipo de respuesta al tipo de petición.
    public class Response<T>
    {
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public List<string> Exceptions { get; set; } = new List<string>();

    }
}
