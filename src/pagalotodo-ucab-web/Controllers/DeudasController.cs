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
using System.Text.Json;
using System.Web.Http.Results;

namespace UCABPagaloTodoWeb.Controllers
{
    public class DeudasController : Controller
    {
        private readonly ILogger<DeudasController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public DeudasController(IHttpService httpService, ILogger<DeudasController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> DeudaConsu()
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            var url = $"consumidores/{username}/Deudas";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            try
            {
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<DeudaResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    return View(resultObject.Data);
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
        public async Task<IActionResult> DeudaPrestador()
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            var url = $"prestadores/{username}/servicios";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var servicios = new ModeloDeudaPresta()
                    {
                        Servicios = resultObject.Data
                    };
                    return View(servicios);
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
        public async Task<IActionResult> DeudaPrestador(ModeloDeudaPresta model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            var url = $"prestadores/{username}/servicios";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var servicios = new ModeloDeudaPresta()
                    {
                        Servicios = resultObject.Data,
                        Deudas = await Deuda(model.ServicioSeleccionado)
                    };
                    return View(servicios);
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

        [HttpGet]
        public async Task<IActionResult> DeudaAdmin()
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            var url = $"servicios/servicio";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<ServiciosResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    var listadeudas = new List<DeudaResponse>();
                    foreach (var servicio in resultObject.Data)
                    {
                        var deudas = await Deuda(servicio.Nombre);
                        if (deudas is not null)
                            foreach (var deuda in deudas)
                                listadeudas.Add(deuda);
                    }
                    return View(listadeudas);
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

        public async Task<List<DeudaResponse>> Deuda(string servicio)
        {
            _logger.LogInformation("DeudasController.Deudas HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            string url = $"deudas/{servicio}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            HttpResponseMessage getData = await httpClient.GetAsync(url);
            string jsonResult = await getData.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<List<DeudaResponse>>>(jsonResult);
            return resultObject.Data;
        }

        [HttpPost]
        public async Task<IActionResult> AddDeuda(ModeloDeudaPresta model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var url = $"deudas";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            var form = GenerateMultiPart.GenerateFile(model.file);
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PostAsync(url, form);
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
            return RedirectToAction("DeudaPrestador", "Deudas");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}