using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    public class ConsultarFormatoQuery : IRequest<List<FormatosResponse>>
    {

        public string servicio = string.Empty;

        public ConsultarFormatoQuery(string _servicio)
        {
            servicio = _servicio;
        }

    }
}
