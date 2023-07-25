using MediatR;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Requests;
using XAct.Authentication;

namespace UCABPagaloTodoMS.Application.Commands
{
    //Descripcion:
    //Clase cuyo constructor recibe la petición de tipo StatusUserRequest.
    public class StatusUserCommand : IRequest<string>
    {
        public StatusUserRequest Request { get; set; }

        public IStatusUserValidator Validation { get; set; }

        public string Username { get; set; }

        public StatusUserCommand(StatusUserRequest request, IStatusUserValidator validation, string username)
        {
            Request = request;
            Validation = validation;
            Username = username;
        }
    }
}
