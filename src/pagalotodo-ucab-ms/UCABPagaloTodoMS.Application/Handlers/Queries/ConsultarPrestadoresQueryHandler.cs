using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    /*
        <summary>
          Handler the ConsultarPrestadorQuery
        </summary>
        <remarks>
             Description
                GET Prestadores
        </remarks>
        <typeparam name="TRequest">ConsultarPrestadorQuery</typeparam>
        <typeparam name="TResponse">List<PrestadoresResponse></typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = List<PrestadoresResponse>
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns a list with the Prestadores</returns>
    */
    public class ConsultarPrestadoresQueryHandler : IRequestHandler<ConsultarPrestadorQuery, List<PrestadoresResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarPrestadorQuery> _logger;

        public ConsultarPrestadoresQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarPrestadorQuery> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /*
       <summary>
           Handle the Request
       </summary>
       <remarks>
            Description
               Handles a Request and check if the request is null
            Parametros
              - request = TRequest to consult a Prestador (ConsultarPrestadorQuery)
               - CancellationToken 
       </remarks>
       <response>
           Accepted:
               - Task<string> =List<PrestadoresResponse>
           Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Prestadores</returns>    
       */
        public Task<List<PrestadoresResponse>> Handle(ConsultarPrestadorQuery request, CancellationToken cancellationToken)
        {
            try
            {

                if (request is null)
                {
                    throw new CustomException(new() { "Disculpe hubo un error intente mas tarde" });
                }
                else
                {

                    return HandleAsync(request.username);
                }

            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe hubo un error intente mas tarde", ex.Message }); }
        }


        /*
       <summary>
           Handles the Request asynchronously
       </summary>
       <remarks>
            Description
               Handles a request, check the information before add
            Parametros
               - _username = string to consult a Prestador (ConsultarPrestadorQuery)
       </remarks>
       <response>
           Accepted:
               - async Task<string> = List<PrestadoresResponse>
            Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Prestadores</returns>    
       */
        private async Task<List<PrestadoresResponse>> HandleAsync(string _username)
        {
            try
            {
                //If _username is null returns all the Prestadores
                if (_username is null)
                {
                    //Consult the Prestadores's Username to find the Prestadores,
                    //MapEntityAResponse change the Prestadores to PrestadoresResponse
                    var result = _dbContext.Prestador.Select(c => PrestadoresMapper.MapEntityAResponse(c));
                    return await result.ToListAsync();
                }
                else
                {
                    //If _username is not null returns just the concrete Prestadores
                    //Consult the Prestadores's Username to find the Prestadores,
                    //MapEntityAResponse change the Prestadores to PrestadoresResponse
                    var result = _dbContext.Prestador.Where(c => c.Username == _username).Select(c => PrestadoresMapper.MapEntityAResponse(c));
                    //If the Result is empty throws a CustomException
                    if (!result.Any())
                        throw new CustomException(new() { "Disculpe el usuario que busca no se encuentra registrado" }, new() { "1475" });

                    return await result.ToListAsync();

                }
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
        }
    }
}
