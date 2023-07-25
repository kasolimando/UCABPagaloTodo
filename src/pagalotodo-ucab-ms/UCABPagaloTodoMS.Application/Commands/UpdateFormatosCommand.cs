using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class UpdateFormatosCommand : IRequest<string>
    {
        public FormatosRequest Request { get; set; }

        public UpdateFormatosCommand(FormatosRequest request)
        {
            Request = request;
        }

        
    }
}
