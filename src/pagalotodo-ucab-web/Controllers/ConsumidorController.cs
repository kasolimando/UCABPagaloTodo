using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using UCABPagaloTodoWeb.Utilities;
using UCABPagaloTodoWeb.Utilities.Mapper;
using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;
using System.Net.Http.Headers;
using System.Text;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.Utilities.GenerateRequests;

namespace UCABPagaloTodoWeb.Controllers
{
    public class ConsumidorController : Controller
    {

        private readonly ILogger<ConsumidorController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public ConsumidorController(IHttpService httpService, ILogger<ConsumidorController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Registro(LoginRequest model)
        {
            _logger.LogInformation("ConsumidorController.Registro HttpPost");
            var url = $"Consumidores";
            try
            {
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync(url, model);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message+ " Inicie Seon para comenzar";
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                }
                
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult ConsuInicio()
        {
            _logger.LogInformation("ConsumidorController.ConsuInicio HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            ViewData["Username"] = username;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CuentaConsu()
        {
            _logger.LogInformation("ConsumidorController.CuentaConsu HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"consumidores/username?username={username}";
            ViewData["Username"] = username;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<ConsumidoresResponse>>>(jsonResult);
                    ViewData["Mensaje"] = resultObject.Message;
                    var Consumidor = ConsumidorMapper.MapResponseARequest(resultObject.Data);
                    TempData["MensajeExito"] = resultObject.Message;
                    return View(Consumidor);
                }
                else
                {
                    // La solicitud no fue exitosa
                    var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;

                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModificarConsu(ConsumidorRequest model)
        {
            _logger.LogInformation("ConsumidorController.ModificarConsu HttpPut");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"consumidores/Update/{username}";
            model.Username = username;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.PutAsJsonAsync(url, model);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                } 
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            ViewData["Username"] = username;
            return RedirectToAction("CuentaConsu", "Consumidor");
        }


        [HttpPost]
        public async Task<IActionResult> CambiarClaveConsu(ConsumidorRequest model)
        {
            _logger.LogInformation("ConsumidorController.CambiarClaveConsu HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"consumidores/CambioClave/{username}";
            var cambioClaveUserRequest = ConsumidorMapper.MapRequestACambioClave(model);
            var request = GeneratePatchRequest.PatchRequestCambioClave(cambioClaveUserRequest, url);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.SendAsync(request);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            ViewData["Username"] = username;
            return RedirectToAction("CuentaConsu", "Consumidor");
        }

        [HttpGet]
        public async Task<IActionResult> Consumidores()
        {
            _logger.LogInformation("ConsumidorController.CuentaConsu HttpGet");
            string url = $"consumidores/username";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<ConsumidoresResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var Consumidor = new ModelConsumidores()
                    {
                        Consumidores = resultObject.Data
                    };
                    return View(Consumidor);
                }
                else
                {
                    // La solicitud no fue exitosa
                    var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;

                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConsu()
        {
            _logger.LogInformation("ConsumidorController.DeleteConsu HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"consumidores/UpdateStatus/{username}";
            var request = GeneratePatchRequest.PatchRequestStatus(url, false);
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.SendAsync(request);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                }
            }catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> StatusConsu(ModelConsumidores model)
        {
            _logger.LogInformation("ConsumidorController.StatusConsu HttpPatch");
            //var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"consumidores/UpdateStatus/{model.Username}";
            var statusRequest = new StatusUserRequest() { Estatus = model.Status };
            string jsonString = JsonConvert.SerializeObject(statusRequest);
            try
            {
                HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = httpContent
                };
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.SendAsync(request);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    _logger.LogInformation("el estatus es: "+ model.Status);
                    TempData["MensajeExito"] = resultObject.Message;
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Consumidores", "Consumidor");
        }
    }
}
