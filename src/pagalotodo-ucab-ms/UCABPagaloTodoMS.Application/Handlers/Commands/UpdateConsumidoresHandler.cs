using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /*
        <summary>
          Handler the UpdateConsumidoresCommand
        </summary>
        <remarks>
             Description
                PUT Consumidores
        </remarks>
        <typeparam name="TRequest">UpdateConsumidoresCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = Username
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns the user's name</returns>
    */

    public class UpdateConsumidoresHandler : IRequestHandler<UpdateConsumCommand, string>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<UpdateConsumCommand> _logger;

         public UpdateConsumidoresHandler(IUCABPagaloTodoDbContext dbContext,ILogger<UpdateConsumCommand> logger)
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
                - TRequest = Request to update a Consumidor (UpdateConsumidoresCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = username
            Denied:
                - CustomException
        </response>
        <returns>Returns the user's name</returns>    
        */

        public Task<string> Handle(UpdateConsumCommand request, CancellationToken cancellationToken)
         {
             try
             {
                 if (request.Request is null)
                 {
                    _logger.LogWarning("UpdateConsumidoresHandler.Handle: Request nulo..");
                    throw new CustomException(new() { "Solicitud Invalida (Update Consumidor)" });
                }
                 else
                 {
                     return HandleAsync(request);
                 }
             }
             catch (Exception)
             {
                _logger.LogWarning("UpdateConsumidoresHandler.Handle: Request nulo..");
                throw new CustomException(new() { "Solicitud Invalida (Update Consumidor)" });
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
               - TRequest = request to update a Consumidor (UpdateConsumidoresCommand)
       </remarks>
       <response>
           Accepted:
               - async Task<string> = username
           Denied:
               - CustomException
       </response>
       <returns>Returns the user's name</returns>    
       */
        private async Task<string> HandleAsync(UpdateConsumCommand request)
         {
             try
             {
                var validator = new UpdateConsumValidator();
                //Check the info's format and is stored on result
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    return await UpdateUserValidation.ValidateUpdateConsumidor(request.Username,request.Request, _dbContext);
                }
                else
                {
                    throw new ValidatorException(result);
                }
            }
             catch (ValidatorException ex)
             {
                throw new CustomException(ex.GetMessages(), ex.GetCodes());
             }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
             {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
             }
             catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }

        }
     }
}
