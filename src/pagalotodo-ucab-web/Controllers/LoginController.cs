using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using UCABPagaloTodoWeb.Services.Interface;
using UCABPagaloTodoWeb.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UCABPagaloTodoWeb.Models.Request;
using UCABPagaloTodoWeb.Models.Response;

namespace UCABPagaloTodoWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;
        private readonly Logger<UsuarioController> loggerUsuario;
        private readonly UsuarioController usuarioController;
        private readonly RoleRedirectorFactory redirector;
        public LoginController(IHttpService httpService, ILogger<LoginController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
            loggerUsuario = new Logger<UsuarioController>(new LoggerFactory());
            usuarioController = new UsuarioController(loggerUsuario);
            redirector = new RoleRedirectorFactory();
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("LoginController.Login HttpGet");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            _logger.LogInformation("LoginController.Login HttpPost");
            string url = $"Login/{model.Username}/{model.Clave}";
            try
            {
                HttpResponseMessage getData = await httpClient.GetAsync(url);
                string jsonResult = await getData.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<LoginResponse>>(jsonResult);
                if (getData.IsSuccessStatusCode)
                {
                    var roleClaim = GenerateCookie(resultObject.Data);
                    var view = redirector.GetRedirector(roleClaim);
                    return view.RedirectToAction(usuarioController);
                }
                else
                {
                    // La solicitud no fue exitosa
                    var errores = string.Join(",", resultObject.Exceptions);
                    TempData["ErrorMessage"] = errores+" Verifique su username y clave";
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Redirigir al usuario a la página de inicio, que no se puede cachear
            return RedirectToAction("Login", "Login");
        }

        public Claim GenerateCookie(LoginResponse response)
        {
            // Crear la cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Solo en conexiones HTTPS
                SameSite = SameSiteMode.None, // Solo en conexiones HTTPS
                Expires = DateTime.Now.AddMinutes(60)
            };
            Response.Cookies.Append("MiCookie", response.TipoUsuario, cookieOptions);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(response.TipoUsuario);

            var claims = jwtToken.Claims;

            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        }
    }
}
