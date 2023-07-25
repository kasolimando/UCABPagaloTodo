using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UCABPagaloTodoWeb.Services.Interface;
using UCABPagaloTodoWeb.Utilities;
using System.Net.Http.Headers;
using UCABPagaloTodoWeb.Utilities.Mapper;
using System.Text;
using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;
using UCABPagaloTodoWeb.Models;
using System.Diagnostics;
using System.Reflection;

namespace UCABPagaloTodoWeb.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ILogger<ServicioController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public ServicioController(IHttpService httpService, ILogger<ServicioController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddServicio(ModelosServicios model)
        {
            _logger.LogInformation("ServicioController.Registro HttpPost");
            var url = $"Servicios";
            try
            {
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync<ServicioRequest>(url, model.ServiciosRequest);
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
            return RedirectToAction("ServicioAdmin", "Servicio");
        }

        [HttpGet]
        public async Task<IActionResult> ServicioAdmin()
        {
            _logger.LogInformation("ServicioController.ServicioAdmin HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"servicios/servicio";
            ViewData["Username"] = username;
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                    var listaFormatos = new List<FormatosResponse>();
                    foreach (var servicio in resultObject.Data)
                    {
                        var formatos = await Formatos(servicio.Nombre);
                        if (formatos is not null)
                        {
                            foreach (var formato in formatos)
                            {
                                listaFormatos.Add(formato);
                            }
                        }
                    }
                    var Model = new ModelosServicios()
                    {
                        ServicioResponse = resultObject.Data,
                        Formatos = listaFormatos
                    };
                    return View(Model);
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModificarServicio(ModelosServicios model)
        {
            _logger.LogInformation("ServicioController.ModificarServicio HttpPut");
            string url = $"servicios/Update/{model.ServiciosRequest.Nombre}";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PutAsJsonAsync<ServicioRequest>(url, model.ServiciosRequest);
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
            return RedirectToAction("ServicioAdmin", "Servicio");
        }

        [HttpPost]
        public async Task<IActionResult> EstatusServicio(ModelosServicios model)
        {
            _logger.LogInformation("ServicioController.EstatusServicio HttpPatch");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"servicios/UpdateStatus/{model.ServiciosRequest.Nombre}";
            var statusRequest = new StatusServicioRequest() { Estatus = model.ServiciosRequest.Estatus };
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
             catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("ServicioAdmin", "Servicio");
        }

        [HttpGet]
        public async Task<IActionResult> ServicioConsu()
        {
            _logger.LogInformation("ServicioController.ServicioConsumidor HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"servicios/servicio";
            ViewData["Username"] = username;
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                    var listaDeudas = new List<DeudaResponse>();
                    foreach (var servicio in resultObject.Data)
                    {
                        var deuda = await Deuda(servicio.Nombre);
                        if (deuda is not null)
                        {
                            listaDeudas.Add(deuda);
                        }
                    }
                    var Model = new ModelosServiciosConsu()
                    {
                        ServicioResponse = resultObject.Data,
                        Deudas = listaDeudas
                    };
                    return View(Model);
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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ServicioPrestador()
        {
            _logger.LogInformation("ServicioController.ServicioConsumidor HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"prestadores/{username}/servicios";
            ViewData["Username"] = username;
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                    var listaFormatos = new List<FormatosResponse>();
                    foreach (var servicio in resultObject.Data)
                    {
                        var formatos = await Formatos(servicio.Nombre);
                        if (formatos is not null)
                        {
                            foreach (var formato in formatos)
                            {
                                listaFormatos.Add(formato);
                            }
                        }
                    }
                    var Model = new ModeloServicioPresta()
                    {
                        Servicios = resultObject.Data,
                        Formatos = listaFormatos
                    };
                    return View(Model);
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
            return View();
        }


        public async Task<DeudaResponse> Deuda(string servicio)
        {
            _logger.LogInformation("DeudasController.Deudas HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"Deudas/{servicio}";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<List<DeudaResponse>>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    return resultObject.Data.Where(d => d.Consumidor == username).FirstOrDefault();
                }
            }
             catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return null;
        }

        public async Task<List<FormatosResponse>> Formatos(string servicio)
        {
            _logger.LogInformation("DeudasController.Deudas HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"formatos/{servicio}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            HttpResponseMessage getData = await httpClient.GetAsync(url);
            string jsonResult = await getData.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<List<FormatosResponse>>>(jsonResult);
            return resultObject.Data;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}