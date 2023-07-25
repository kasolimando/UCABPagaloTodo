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
    public class ReporteController : Controller
    {
        private readonly ILogger<ReporteController> _logger;
        private readonly IHttpService _httpService;
        private readonly HttpClient httpClient;
        private string username;

        public ReporteController(IHttpService httpService, ILogger<ReporteController> logger)
        {
            _httpService = httpService;
            httpClient = _httpService.GetConnection();
            _logger = logger;
        }
        public IActionResult Reporte()
        {
            _logger.LogInformation("ReporteController.Reporte HttpGet");
            return View();
        }

        
    }
}
