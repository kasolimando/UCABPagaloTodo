using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Responses;
using XAct.Users;

namespace UCABPagaloTodoMS.Application.Commands
{

    public class RecuperarClaveCommand : IRequest<RecuperarClaveResponse>
    {
        public string username = string.Empty;
        public RecuperarClaveCommand(string _username)
        {
            username = _username;
        }
    }
}
