using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public class AgregarDeudaCommand : IRequest<string>
        {
            public IFormFile Archivo { get; set; }

            public AgregarDeudaCommand(IFormFile archivo)
            {
                Archivo = archivo; 
            }
        }
    
}
