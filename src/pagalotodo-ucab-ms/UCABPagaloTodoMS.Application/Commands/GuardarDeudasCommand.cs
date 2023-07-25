using MediatR;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class GuardarDeudasCommand : IRequest<Guid>
    {
        public GuardarDeudaRequest Request { get; set; }

        public GuardarDeudasCommand(GuardarDeudaRequest request)
        {
            Request = request;
        }
    }
}
