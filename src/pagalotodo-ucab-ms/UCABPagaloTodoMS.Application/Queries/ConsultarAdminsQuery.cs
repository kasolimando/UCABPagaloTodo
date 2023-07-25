using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    //Petición de consulta que extiende de la interfaz IRequest del paquete MediatR.
    public class ConsultarAdminsQuery : IRequest<AdminsResponse>
    {
        public string? username = string.Empty;

        public ConsultarAdminsQuery(string? username)
        {
            this.username = username;
        }
    }
}

