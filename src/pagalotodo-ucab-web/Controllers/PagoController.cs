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
using System.Collections.Generic;

namespace UCABPagaloTodoWeb.Controllers
{
    public class PagoController : Controller
    {
        private readonly ILogger<PagoController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;

        public PagoController(IHttpService httpService, ILogger<PagoController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RealizarPago(ModelosServiciosConsu model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var url = $"pagos";
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            model.Pago.Consumidor = username;
            model.Pago.Servicio = model.ServicioSeleccionado;
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync<PagoRequest>(url, model.Pago);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);

                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message + " Inicie Sesion";
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
            return RedirectToAction("ServicioConsu", "Servicio");
        }

        [HttpGet]
        public IActionResult PagoAdmin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PagoAdmin(ModeloPagoPresta model)
        {
            var pagos = await Pagos(model);
            if (pagos is not null)
            {
                var servicios = new ModeloPagoPresta()
                {
                    Pagos = pagos
                };
                return View(servicios);
            }
            return View();
        }

        [HttpGet]
        public IActionResult PagoConsu()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PagoPresta()
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
                    var servicios = new ModeloPagoPresta()
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
        public async Task<IActionResult> PagoConsu(ModeloPagoConsu model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var url = $"pagos/consultarpagos";
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            model.Consulta.Consumidor = username;
            model.Consulta.fechaFin = FixDate.FixFormatData(model.Consulta.fechaFin);
            model.Consulta.fechaInicio = FixDate.FixFormatData(model.Consulta.fechaInicio);
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PostAsJsonAsync<ConsultarPagosRequest>(url, model.Consulta);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                if (getData.IsSuccessStatusCode)
                {
                    var resultObject = JsonConvert.DeserializeObject<Response<List<PagoResponse>>>(jsonResult);
                    TempData["MensajeExito"] = resultObject.Message;
                    return View(new ModeloPagoConsu() { Pagos = resultObject.Data });
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
        public async Task<IActionResult> PagoPresta(ModeloPagoPresta model)
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
                    TempData["MensajeExito"] = resultObject.Message + " Inicie Sesion";
                    var servicios = new ModeloPagoPresta()
                    {
                        Servicios = resultObject.Data,
                        Pagos = await Pagos(model)
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

        public async Task<List<PagoResponse>> Pagos(ModeloPagoPresta model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var url = $"pagos/consultarpagos";
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            model.Consulta.fechaFin = FixDate.FixFormatData(model.Consulta.fechaFin);
            model.Consulta.fechaInicio = FixDate.FixFormatData(model.Consulta.fechaInicio);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            HttpResponseMessage getData = await httpClient.PostAsJsonAsync<ConsultarPagosRequest>(url, model.Consulta);
            string jsonResult = await getData.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<List<PagoResponse>>>(jsonResult);
            return resultObject.Data;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> AddPagoConfirmacion(ModeloPagoPresta model)
        {
            _logger.LogInformation("PagoController.RealizarPago HttpPost");
            var url = $"conciliaciones";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                var form = new MultipartFormDataContent();
                var fileContent = new StreamContent(model.file.OpenReadStream());
                form.Add(fileContent, "Archivo", model.file.FileName);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
                HttpResponseMessage getData = await httpClient.PostAsync(url, form);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message + " Inicie Sesion";
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
            return RedirectToAction("PagoPresta", "Pago");
        }
    }
}