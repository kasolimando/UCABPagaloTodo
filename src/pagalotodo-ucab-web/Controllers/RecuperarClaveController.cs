using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using UCABPagaloTodoWeb.Utilities;
using System.Net.Http.Headers;
using UCABPagaloTodoWeb.Utilities.Mapper;
using System.Text;
using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Utilities.GenerateRequests;


namespace UCABPagaloTodoWeb.Controllers
{
    public class RecuperarClaveController : Controller
    {
        private readonly ILogger<RecuperarClaveController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public RecuperarClaveController(IHttpService httpService, ILogger<RecuperarClaveController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarContra(LoginRequest model)
        {
            _logger.LogInformation("CierreController.CierreContable HttpPost");
            string errores;
            var url = $"recuperarclave/{model.Nombre}";
            try
            {
                var request = GeneratePatchRequest.PatchRequestRecuperarClave(model.Nombre, url);
                HttpResponseMessage getData = await httpClient.SendAsync(request);
                var jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);

                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                }
                else
                {
                    // La solicitud no fue exitosa
                    errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Login", "Login");
        }
    }
}
