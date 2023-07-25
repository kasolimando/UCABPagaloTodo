using Microsoft.AspNetCore.Mvc;

namespace UCABPagaloTodoWeb.Services.Interface
{
    public interface IRoleRedirector
    {
        IActionResult RedirectToAction(Controller controller);
    }
}
