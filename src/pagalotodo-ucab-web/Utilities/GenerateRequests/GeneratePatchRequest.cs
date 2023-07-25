using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;
using UCABPagaloTodoWeb.Models.Request;

namespace UCABPagaloTodoWeb.Utilities.GenerateRequests
{
    public static class GeneratePatchRequest
    {
        public static HttpRequestMessage PatchRequestStatus(string url, bool status)
        {
            var statusRequest = new StatusUserRequest() { Estatus = false };
            string jsonString = JsonConvert.SerializeObject(statusRequest);
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = httpContent
            };
            return request;
        }

        public static HttpRequestMessage PatchRequestCambioClave(CambioClaveUserRequest claveRequest, string url)
        {
            string jsonString = JsonConvert.SerializeObject(claveRequest);
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = httpContent };
            return request;
        }


        public static HttpRequestMessage PatchRequestRecuperarClave(string username, string url)
        {
            HttpContent httpContent = new StringContent(username, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = httpContent };
            return request;
        }
    }
}
