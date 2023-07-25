﻿using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoWeb.Services.Interface;

namespace UCABPagaloTodoWeb.Services.Implementation
{
    public class AdminRedirector : IRoleRedirector
    {
        public IActionResult RedirectToAction(Controller controller)
        {
            return controller.RedirectToAction("AdminInicio", "Administrador");
        }
    }
}
