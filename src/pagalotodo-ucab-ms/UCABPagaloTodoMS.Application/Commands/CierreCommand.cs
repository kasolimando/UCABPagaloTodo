using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class CierreCommand : IRequest<string>
    {
        public string Servicio { get; set; }

        public CierreCommand(string servicio)
        {
            Servicio = servicio;
        }
    }
}
