using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;


namespace UCABPagaloTodoMS.Application.Requests
{
    [ExcludeFromCodeCoverage]
    public class CargarDeudaRequest
    {
        public IFormFile archivo { get; set; }

    }
}
