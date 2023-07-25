using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class UpdateConsumCommand : IRequest<string>
    {
        public ConsumidorRequest Request { get; set; }
        public string Username { get; set; } = string.Empty;

        public UpdateConsumCommand(ConsumidorRequest request, string username)
        {
            Request = request;
            Username = username;
        }

        
    }
}
