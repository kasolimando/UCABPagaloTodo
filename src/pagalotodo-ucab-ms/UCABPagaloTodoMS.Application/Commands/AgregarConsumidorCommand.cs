using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    //Descripcion:
    //Clase cuyo constructor recibe la petición de tipo ConsumidorRequest.
    public class AgregarConsumidorCommand : IRequest<string>
        {
            public ConsumidorRequest Request { get; set; }

            public AgregarConsumidorCommand(ConsumidorRequest request)
            {
                Request = request; 
            }
        }
    
}
