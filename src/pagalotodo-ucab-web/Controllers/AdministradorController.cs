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
    public class AdministradorController : Controller
    {
        private readonly ILogger<AdministradorController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;
        private string username;

        public AdministradorController(IHttpService httpService, ILogger<AdministradorController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> CuentaAdmin()
        {
            _logger.LogInformation("AdministradorController.CuentaAdmin HttpGet");
            username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            string url = $"administradores/{username}";
            try
            {
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<AdminsResponse>>(jsonResult);
                ViewData["Username"] = username;
                if (getData.IsSuccessStatusCode)
                {
                    TempData["MensajeExito"] = resultObject.Message;
                    var Administrador = AdminMapper.MapResponseARequest(resultObject.Data);
                    return View(Administrador);
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
        public async Task<IActionResult> ModificarAdmin(AdminsRequest model)
        {
            _logger.LogInformation("AdministradorController.ModificarAdmin HttpPut");
            username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            string url = $"administradores/Update/{username}";
            HttpResponseMessage getData = await httpClient.PutAsJsonAsync(url, model);
            string jsonResult = await getData.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
            try
            {
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
            return RedirectToAction("CuentaAdmin", "Administrador");
        }


        [HttpPost]
        public async Task<IActionResult> CambiarClaveAdmin(AdminsRequest model)
        {
            _logger.LogInformation("AdministradorController.CambiarClaveAdmin HttpPatch");
            username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            string url = $"administradores/CambioClave/{username}";
            var cambioClaveUserRequest = AdminMapper.MapRequestACambioClave(model);
            var request = GeneratePatchRequest.PatchRequestCambioClave(cambioClaveUserRequest,url);
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
            return RedirectToAction("CuentaAdmin", "Administrador");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin()
        {
            _logger.LogInformation("AdministradorController.DeleteAdmin HttpPatch");
            username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["MiCookie"]);
            string url = $"administradores/UpdateStatus/{username}";
            var request = GeneratePatchRequest.PatchRequestStatus(url, false);
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
                    TempData["ErrorMessage"] = errores + " Verifique su username y contraseña";
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Login", "Login");
        }


        [HttpGet]
        public IActionResult AdminInicio()
        {
            _logger.LogInformation("AdministradorController.AdminInicio HttpGet");
            var username = DecodeToken.DecodeTokeUsername(Request.Cookies["MiCookie"]);
            ViewData["Username"] = username;
            return View();
        }
    }
}
