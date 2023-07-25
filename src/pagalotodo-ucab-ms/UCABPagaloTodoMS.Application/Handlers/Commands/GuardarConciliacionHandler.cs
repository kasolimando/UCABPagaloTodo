using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /*
       <summary>
         Handler the GuardarConciliacionCommand
       </summary>
       <remarks>
            Description
               POST Formatos
       </remarks>
       <typeparam name="TRequest">GuardarConciliacionCommand</typeparam>
       <typeparam name="TResponse">Guid</typeparam>
       <response>
           Response for a request
           Accepted:
               - Task<Guid> = the Servicio's Guid
           Failed:
               - Error en formato de campos = ValidatorException
               - Error de data = SQLException
               - Resto de errores = Exception
               Retorna los tres errores en el tipo CustomException
       </response>
       <returns>Returns the Servicio's Guid</returns>
   */
    public class GuardarConciliacionHandler : IRequestHandler<GuardarConciliacionCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<GuardarConciliacionHandler> _logger;

        public GuardarConciliacionHandler(IUCABPagaloTodoDbContext dbContext, ILogger<GuardarConciliacionHandler> logger)
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
               - TRequest = Request to storage a deuda (GuardarDeudasCommand)
               - CancellationToken 
       </remarks>
       <response>
           Accepted:
               - Task<string> = Servicio
           Failed:
               - CustomException
       </response>
       <returns>Returns the Servicio's Guid</returns>    
       */

        public async Task<Guid> Handle(GuardarConciliacionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Request.Servicio))
                {

                    throw new CustomException(new() { "Solicitud Invalida" });
                }
                else
                {
                    return await HandleAsync(request.Request);
                }
            }
            catch(Exception)
            {
                _logger.LogWarning("Error en GuardarConciliacionHandler.Handle ");
                return Guid.NewGuid();
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
                - TRequest = request to storage a Deuda (GuardarConciliacionCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Servicio
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's Guid</returns>    
        */


        private async Task<Guid> HandleAsync(GuardarConciliacionRequest request)
        {
            try
            {
                _logger.LogInformation("GuardarConciliacionHandler");
                return await ConciliacionValidation.ProcesarConciliacion(_dbContext, request);
            }
            catch (Exception)
            {
                _logger.LogWarning("Error en GuardarConciliacionHandler.HandleAsync ");
                return Guid.NewGuid();
            }
        }
    }
}
