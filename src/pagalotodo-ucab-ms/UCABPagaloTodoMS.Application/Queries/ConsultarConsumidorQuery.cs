using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    public class ConsultarConsumidorQuery : IRequest<List<ConsumidoresResponse>>
    {

        public string username = string.Empty;

        public ConsultarConsumidorQuery(string _username)
        {
            username = _username;
        }

    }
}
