using MediatR;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public class GuardarConciliacionCommand : IRequest<Guid>
        {
            public GuardarConciliacionRequest Request { get; set; }

            public GuardarConciliacionCommand(GuardarConciliacionRequest request)
            {
                Request = request; 
            }
        }
    
}
