using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.BusinessValidation;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    /*
        <summary>
          Handler the ConsultarFormatoQuery
        </summary>
        <remarks>
             Description
                GET Formatos
        </remarks>
        <typeparam name="TRequest">ConsultarFormatoQuery</typeparam>
        <typeparam name="TResponse">List<FormatosResponse></typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = List<FormatosResponse>
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns a list with the Formatos</returns>
    */
    public class ConsultarFormatosQueryHandler : IRequestHandler<ConsultarFormatoQuery, List<FormatosResponse>>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<ConsultarFormatoQuery> _logger;

         public ConsultarFormatosQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarFormatoQuery> logger)
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
               - request = TRequest to consult a Formato (ConsultarFormatoQuery)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = List<FormatosResponse>
            Failed:
                - CustomException
        </response>
        <returns>Returns a list with the Formatos</returns>    
        */
        public Task<List<FormatosResponse>> Handle(ConsultarFormatoQuery request, CancellationToken cancellationToken)
         {
             try
             {
                
                 if (request is null)
                 {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" });
                }
                 else
                 {
                    
                     return HandleAsync(request.servicio);
                 }

             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
         }

        /*
       <summary>
           Handles the Request asynchronously
       </summary>
       <remarks>
            Description
               Handles a request, check the information before add
            Parametros
               - _servicio = string to consult a Format (ConsultarFormatoQuery)
       </remarks>
       <response>
           Accepted:
               - async Task<string> = List<FormatosResponse>
            Failed:
               - CustomException
       </response>
       <returns>Returns a list with the Formatos</returns>    
       */
        private async Task<List<FormatosResponse>> HandleAsync(string servicioNombre)
         {
             try
             {
                //Get the Guid of the Servicio's name _servicio
                var servicio = await ServicioValidation.GetServicio(servicioNombre, _dbContext);
                var result = _dbContext.FormatoServicio.Include(fs => fs.FormatoCon).Where(fs => fs.ServicioEntityId == servicio.Id).Select(f => FormatoMapper.MapEntityAResponse(f.FormatoCon.NombreCampo,f.Logitud, f.Requerido, servicio.Nombre));
                //If the Result is empty throws a CustomException
                if (!result.Any())
                    throw new CustomException(new() { "El servicio no cuenta con un formato registrado" }, new() { "1475" });
                //else returns the FormatosResponse
                return await result.ToListAsync();

            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message }); }
        }
     }
}
