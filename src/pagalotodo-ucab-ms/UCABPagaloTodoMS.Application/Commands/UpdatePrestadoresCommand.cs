using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands
{
    public class UpdatePrestadoresCommand : IRequest<string>
    {
        public PrestadorRequest Request { get; set; }
        public string Username { get; set; } = string.Empty;

        public UpdatePrestadoresCommand(PrestadorRequest request, string username)
        {
            Request = request;
            Username = username;
        }

        
    }
}
