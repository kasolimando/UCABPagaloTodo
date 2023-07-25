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
          Handler the UpdatePrestadoresCommand
        </summary>
        <remarks>
             Description
                PUT Prestadores
        </remarks>
        <typeparam name="TRequest">UpdatePrestadoresCommand</typeparam>
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
        <returns>Returns the username</returns>
    */
    public class UpdatePrestadoresHandler : IRequestHandler<UpdatePrestadoresCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<UpdatePrestadoresCommand> _logger;

        public UpdatePrestadoresHandler(IUCABPagaloTodoDbContext dbContext, ILogger<UpdatePrestadoresCommand> logger)
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
               - TRequest = Request to updated a Prestador (UpdatePrestadoresCommand)
               - CancellationToken 
       </remarks>
       <response>
           Accepted:
               - Task<string> = userame
           Failed:
               - CustomException
       </response>
       <returns>Returns the username</returns>    
       */
        public Task<string> Handle(UpdatePrestadoresCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Request is null)
                {
                    throw new CustomException(new() { "Solicitud Invalida (Update Consumidor)" });
                }
                else
                {
                    return HandleAsync(request);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(new() { "Solicitud Invalida (Update Consumidor) ", ex.Message });
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
                - TRequest = request to updated a Prestador (UpdatePrestadoresCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Servicio
            Denied:
                - CustomException
        </response>
        <returns>Returns the username</returns>    
        */
        private async Task<string> HandleAsync(UpdatePrestadoresCommand request)
        {
            try
            {
                //A instance to check the info's format with the Validator
                var validator = new UpdatePrestadorValidator();
                //Check the info's format and is stored on result
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    var updateUserValidation = new UpdateUserValidation(_dbContext);
                    return await updateUserValidation.ValidateUpdatePrestador(request.Request,request.Username);
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
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe hubo un error intente mas tarde ", ex.Message });
            }
        }
    }
}
