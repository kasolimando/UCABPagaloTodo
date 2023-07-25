using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using UCABPagaloTodoWeb.Utilities;
using UCABPagaloTodoWeb.Utilities.Mapper;
using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models;
using System.Net.Http.Headers;
using System.Text;


namespace UCABPagaloTodoWeb.Controllers
{
    public class PrestadorController : Controller
    {
        private readonly ILogger<PrestadorController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public PrestadorController(IHttpService httpService, ILogger<PrestadorController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpGet]
        public IActionResult PrestadorInicio()
        {
            _logger.LogInformation("PrestadorController.PrestadorInicio HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            ViewData["Username"] = username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(ModelPrestadores model)
        {
            _logger.LogInformation("ConsumidorController.Registro HttpPost");
            var url = $"prestadores";
            try
            {
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync<PrestadorRequest>(url, model.PrestadorRequest);
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
            return RedirectToAction("Prestadores", "Prestador");
        }

        [HttpGet]
        public async Task<IActionResult> Prestadores()
        {
            _logger.LogInformation("PrestadorController.CuentaPrestador HttpGet");
            string url = $"prestadores/username";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<PrestadoresResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var Prestador = new ModelPrestadores()
                    {
                        Prestadores = resultObject.Data
                    };
                    return View(Prestador);
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

        [HttpGet]
        public async Task<IActionResult> CuentaPrestador()
        {
            _logger.LogInformation("PrestadorController.CuentaPrestador HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/username?username={username}";
            ViewData["Username"] = username;
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<PrestadoresResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var Prestador = PrestadorMapper.MapResponseARequest(resultObject.Data);
                    return View(Prestador);
                }
                else
                {
                    // La solicitud no fue exitosa
                    var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                    var errores = string.Join(",", resultObject.Exceptions);

                    TempData["ErrorMessage"] = errores;

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModificarPrestador(ModelPrestadores model)
        {
            _logger.LogInformation("PrestadorController.ModificarPrestador HttpPut");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/UpdatePrestador/{model.PrestadorRequest.Username}";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PutAsJsonAsync<PrestadorRequest>(url, model.PrestadorRequest);
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            ViewData["Username"] = username;
            return RedirectToAction("Prestadores", "Prestador");
        }


        [HttpPost]
        public async Task<IActionResult> CambiarClavePrestador(PrestadorRequest model)
        {
            _logger.LogInformation("PrestadorController.CambiarClavePrestador HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/CambioClave/{username}";
            var cambioClaveUserRequest = PrestadorMapper.MapRequestACambioClave(model);
            string jsonString = JsonConvert.SerializeObject(cambioClaveUserRequest);
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
            ViewData["Username"] = username;
            return RedirectToAction("CuentaPrestador", "Prestador");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrestador()
        {
            _logger.LogInformation("PrestadorController.DeletePrestador HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/UpdateStatus/{username}";
            var statusRequest = new StatusUserRequest() { Estatus = false };
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
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> StatusPrestador(ModelPrestadores model)
        {
            _logger.LogInformation("PrestadorController.StatusPrestador HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/UpdateStatus/{model.Username}";
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
            return RedirectToAction("Prestadores", "Prestador");
        }
    }
}
