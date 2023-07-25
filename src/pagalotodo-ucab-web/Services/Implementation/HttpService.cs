using System.Net.Http.Headers;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.Services.Interface;

namespace UCABPagaloTodoWeb.Services.Implementation
{
    public class HttpService :IHttpService
    {
        private readonly IConfiguration _configuration;

        public HttpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public HttpClient GetConnection()
        {
            var client = new HttpClient();
            var baseUri = _configuration["BaseUri"];
            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            return client;
        }
    }
}
