using MediatR;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Requests;
namespace UCABPagaloTodoMS.Application.Commands
{
    //Descripcion:
    //Clase cuyo constructor recibe la petición de tipo CambioClaveUserRequest.
    public class CambioClaveUserCommand : IRequest<string>
    {
        public CambioClaveUserRequest Request { get; set; }
        public ICambiarClaveUser Validation { get; set; }
        public string Username { get; set; } = string.Empty;

        public CambioClaveUserCommand(CambioClaveUserRequest _Request, ICambiarClaveUser validation, string username)
        {
            Request = _Request;
            Validation = validation;
            Username = username;
        }
    }
}
