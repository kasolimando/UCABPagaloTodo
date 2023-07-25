using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoWeb.Services.Interface;

namespace UCABPagaloTodoWeb.Services.Implementation
{
    public class ConsumidorRedirector : IRoleRedirector
    {
        public IActionResult RedirectToAction(Controller controller)
        {
            return controller.RedirectToAction("ConsuInicio", "Consumidor");
        }
    }
}
