using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    
    public class LoginUsuariosQuery : IRequest<LoginResponse>
    {
        public string username = string.Empty;
        public string clave = string.Empty;

        public LoginUsuariosQuery(string _username, string _clave)
        {
            username = _username;
            clave = _clave;
        }
    }
}

