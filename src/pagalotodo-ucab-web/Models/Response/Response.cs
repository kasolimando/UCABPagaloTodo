using System.Net;

namespace UCABPagaloTodoWeb.Models.Response
{
    public class Response<T>
    {
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public List<string> Exceptions { get; set; }
    }
}
