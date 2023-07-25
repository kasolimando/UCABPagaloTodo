using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Core.Entities;
using XAct;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    /*
        <summary>
          Handler the ConsultarDeudasQuery
        </summary>
        <remarks>
             Description
                GET Deudas
        </remarks>
        <typeparam name="TRequest">ConsultarDeudasQuery</typeparam>
        <typeparam name="TResponse">List<DeudasResponse></typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = List<DeudasResponse>
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns a list with the Formatos</returns>
    */
    public class ConsultarDeudasQueryHandler : IRequestHandler<ConsultarDeudasQuery, List<DeudaResponse>>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarDeudasQuery> _logger;

         public ConsultarDeudasQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarDeudasQuery> logger)
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
               - request = TRequest to consult a Deuda (ConsultarDeudasQuery)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = List<DeudaResponse>
            Failed:
                - CustomException
        </response>
        <returns>Returns a list with the Deudas</returns>    
        */
        public Task<List<DeudaResponse>> Handle(ConsultarDeudasQuery request, CancellationToken cancellationToken)
         {
             try
             {
                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" });
                }
                 else
                 {
                    
                     return HandleAsync(request);
                 }

             }
             catch (Exception)
             {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" });
            }
         }

        /*
       <summary>
           Handles the Request asynchronously
       </summary>
       <remarks>
            Description
               Handles a request, check the information before add
            Parametros
               - _servicio = string to consult a Deudas (ConsultarDeudasQuery)
       </remarks>
       <response>
           Accepted:
               - async Task<string> = List<DeudaResponse>
            Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Deudas</returns>    
       */
        private async Task<List<DeudaResponse>> HandleAsync(ConsultarDeudasQuery request)
        {
            try
            {
                IQueryable<DeudaResponse> result;
                if (request.usuario.IsNullOrEmpty())
                {
                    var servicio = await ServicioValidation.GetServicio(request.servicio, _dbContext);
                    //MapEntityAResponse change the DeudaEntity to DeudaResponse
                    result = _dbContext.Deuda.Where(f => f.servicioId == servicio.Id).Select(c => DeudasMapper.MapEntityAResponse(c, request.servicio));
                    //If the Result is empty throws a CustomException
                    if (!result.Any())
                        throw new CustomException(new() { "Disculpe el servicio no cuenta con deudas" }, new() { "1475" });
                }
                else
                {
                    result = _dbContext.Deuda.Include(fs => fs.Servicio).Where(f => f.Username == request.usuario).Select(c => DeudasMapper.MapEntityAResponse(c, c.Servicio.Nombre));
                    //If the Result is empty throws a CustomException
                    if (!result.Any())
                        throw new CustomException(new() { "Disculpe el consumidor no cuenta con deudas para ese servicio" }, new() { "1475" });
                }
                return await result.ToListAsync();
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (Exception){throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" });}
        }
    }
}
