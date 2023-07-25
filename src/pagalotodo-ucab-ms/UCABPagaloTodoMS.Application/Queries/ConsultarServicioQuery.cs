using MediatR;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    public class ConsultarServicioQuery : IRequest<List<ServiciosResponse>>
    {

        public string servicio = string.Empty;
        public string tipo = string.Empty;

        public ConsultarServicioQuery(string _servicio, string _tipo)
        {
            servicio = _servicio;
            tipo = _tipo;
        }

    }
}
