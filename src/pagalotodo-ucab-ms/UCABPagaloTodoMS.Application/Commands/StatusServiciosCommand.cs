using MediatR;
using UCABPagaloTodoMS.Application.Requests;
namespace UCABPagaloTodoMS.Application.Commands
{
    //Descripcion:
    //Clase cuyo constructor recibe la petición de tipo .
    public class StatusServiciosCommand : IRequest<string>
    {
        public StatusServicioRequest Request { get; set; }
        public string Servicio { get; set; } = string.Empty;

        public StatusServiciosCommand(StatusServicioRequest _request, string servicio)
        {
            Request = _request;
            Servicio = servicio;
        }
    }
}
