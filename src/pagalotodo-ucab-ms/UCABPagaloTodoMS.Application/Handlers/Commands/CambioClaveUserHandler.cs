using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.BusinessValidation;
using FluentValidation;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{

    /*
        <summary>
          Handler the StatusUserCommand
        </summary>
        <remarks>
             Description
                PUT Status
        </remarks>
        <typeparam name="TRequest">StatusUserCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = username
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns the username of the updated status</returns>
    */
    public class CambioClaveUserHandler : IRequestHandler<CambioClaveUserCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CambioClaveUserCommand> _logger;

        public CambioClaveUserHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CambioClaveUserCommand> logger)
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
                - TRequest = Request to update the status (StatusUserCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = username
            Denied:
                - CustomException
        </response>
        <returns>Returns the username of the updated status</returns>    
        */

        public Task<string> Handle(CambioClaveUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Request is null)
                {
                    throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde" }); ;
                }
                else
                {
                    return HandleAsync(request);
                }
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }


        /*
        <summary>
            Handles the Request asynchronously
        </summary>
        <remarks>
             Description
                Handles a request, check the information before add
             Parametros
                - TRequest = Request to update the status (StatusUserCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = username
            Denied:
                - CustomException
        </response>
        <returns>Returns the username of the updated status</returns>    
        */
        private async Task<string> HandleAsync(CambioClaveUserCommand request)
        {
            try
            {
                var validator = new CambiarClaveValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    var validation = request.Validation;
                    return await validation.ValidateCambioClaveUser(request, _dbContext);
                }
                else
                {
                    throw new ValidatorException(result);
                }
            }
            catch (ValidatorException ex)
            {
                throw new CustomException(ex.GetMessages());
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage(), ex.GetErrorCode());
            }
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe hubo un error intente mas tarde ", ex.Message });
            }

        }
    }
}
