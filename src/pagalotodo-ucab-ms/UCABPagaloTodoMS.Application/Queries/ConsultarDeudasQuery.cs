using MediatR;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class ConsultarDeudasQuery : IRequest<List<DeudaResponse>>
    {

        public string servicio = string.Empty;
        public string usuario = string.Empty;
        public ConsultarDeudasQuery(string _servicio, string _usuario)
        {
            servicio = _servicio;
            usuario = _usuario;
        }

    }
}
