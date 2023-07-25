using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using System.Net.Http.Headers;
using UCABPagaloTodoWeb.Utilities.Mapper;
using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class FormatoController : Controller
    {
        private readonly ILogger<FormatoController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public FormatoController(IHttpService httpService, ILogger<FormatoController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> EditarFormato(ModelosServicios model)
        {
            _logger.LogInformation("FormatoController.EditarFormato HttpPost");
            string errores;
            var url = $"formatos/update/{model.Formatos[0].Servicio}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            var listRequets = FormatoMapper.MapUpdateListResponseToRequest(model.Formatos);
            try
            {
                foreach(var request in listRequets)
                {
                    HttpResponseMessage getData = await httpClient.PutAsJsonAsync(url, request);
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
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("ServicioAdmin", "Servicio");
        }

        [HttpPost]
        public async Task<IActionResult> AddFormato(ModelosServicios model)
        {
            _logger.LogInformation("FormatoController.AddFormato HttpPost");
            string errores;
            var url = $"formatos";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            var request = FormatoMapper.MapListResponseToRequest(model.Formatos);
            try
            {
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync(url, request);
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
