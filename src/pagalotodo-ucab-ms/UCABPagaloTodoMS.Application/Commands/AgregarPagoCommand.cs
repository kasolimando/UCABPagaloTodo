using MediatR;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class AgregarPagoCommand : IRequest<string>
    {
        public PagoRequest Request { get; set; }

        public AgregarPagoCommand(PagoRequest request)
        {
            Request = request;
        }
    }
}
