using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class UpdateServiciosCommand : IRequest<string>
    {
        public ServicioRequest Request { get; set; }

        public UpdateServiciosCommand(ServicioRequest request)
        {
            Request = request;
        }

        
    }
}
