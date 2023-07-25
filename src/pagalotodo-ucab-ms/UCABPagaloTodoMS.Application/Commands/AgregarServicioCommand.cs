using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
   
        public class AgregarServicioCommand : IRequest<string>
        {
            public ServicioRequest Request { get; set; }

            public AgregarServicioCommand(ServicioRequest request)
            {
                Request = request; 
            }
        }
    
}
