using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands
{
    //Descripcion:
    //Clase cuyo constructor recibe la petición de tipo AdminsRequest.

    public class UpdateAdminsCommand : IRequest<string>
    {
        public AdminsRequest Request { get; set; }

        public string Username { get; set; }

        public UpdateAdminsCommand(AdminsRequest request, string username)
        {
            Request = request;
            Username = username;    
        }



    }
}
