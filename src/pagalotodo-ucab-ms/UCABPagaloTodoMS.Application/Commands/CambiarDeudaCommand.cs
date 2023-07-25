using MediatR;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class CambiarDeudaCommand : IRequest<string>
    {
        public string Username { get; set; }

        public string Monto { get; set; }

        public string Servicio { get; set; }

        public CambiarDeudaCommand(string _username, string _monto, string _servicio)
        {
            Username = _username;
            Monto = _monto;
            Servicio = _servicio;
        }
    }
}
