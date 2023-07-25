using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    public class ConsultarPrestadorQuery : IRequest<List<PrestadoresResponse>>
    {

        public string username = string.Empty;

        public ConsultarPrestadorQuery(string _username)
        {
            username = _username;
        }
    }
}
