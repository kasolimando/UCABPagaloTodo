using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public class CargarConciliacionCommand : IRequest<string>
    {
            public IFormFile Archivo { get; set; }

            public CargarConciliacionCommand(IFormFile _archivo)
            {
                    Archivo = _archivo; 
            }
    }
    
}
