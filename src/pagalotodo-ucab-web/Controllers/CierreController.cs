using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using System.Net.Http.Headers;
using System.Text;
using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class CierreController : Controller
    {
        private readonly ILogger<CierreController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public CierreController(IHttpService httpService, ILogger<CierreController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CierreContable(ModelosServicios model)
        {
            _logger.LogInformation("CierreController.CierreContable HttpPost");
            string errores;
            var url = $"cierres?servicio={model.ServiciosRequest.Nombre}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.PostAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
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
            return RedirectToAction("ServicioAdmin", "Servicio");
        }
    }
}
