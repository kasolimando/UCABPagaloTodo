using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
   
        public class AgregarPrestadorCommand : IRequest<string>
        {
            public PrestadorRequest Request { get; set; }

            public AgregarPrestadorCommand(PrestadorRequest _request)
            {
                Request = _request; 
            }
        }
    
}
