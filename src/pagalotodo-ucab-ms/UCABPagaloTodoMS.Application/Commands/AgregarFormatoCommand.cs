using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
   
        public class AgregarFormatoCommand : IRequest<string>
        {
            public FormatosRequest Request { get; set; }

            public AgregarFormatoCommand(FormatosRequest request)
            {
                Request = request; 
            }
        }
    
}
