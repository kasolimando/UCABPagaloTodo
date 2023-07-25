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
         Handler the GuardarDeudasCommand
       </summary>
       <remarks>
            Description
               POST Formatos
       </remarks>
       <typeparam name="TRequest">GuardarDeudasCommand</typeparam>
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
    public class GuardarDeudasHandler : IRequestHandler<GuardarDeudasCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<GuardarDeudasHandler> _logger;

        public GuardarDeudasHandler(IUCABPagaloTodoDbContext dbContext, ILogger<GuardarDeudasHandler> logger)
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

        public async Task<Guid> Handle(GuardarDeudasCommand request, CancellationToken cancellationToken)
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
                _logger.LogWarning("Error en GuardarDeudasHandler.Handle ");
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
                - TRequest = request to storage a Deuda (GuardarDeudasCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Servicio
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's Guid</returns>    
        */


        private async Task<Guid> HandleAsync(GuardarDeudaRequest request)
        {
            try
            {
                _logger.LogInformation("GuardarDeudasHandler");
                return await DeudasValidation.GuardarDeuda(_dbContext, request);
            }
            catch (Exception)
            {
                _logger.LogWarning("Error en GuardarDeudasHandler.HandleAsync ");
                return Guid.NewGuid();
            }
        }
    }
}
