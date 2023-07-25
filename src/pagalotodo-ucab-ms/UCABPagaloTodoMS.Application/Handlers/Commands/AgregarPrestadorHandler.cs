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
          Handler para el registro de nuevos prestador
        </summary>
        <remarks>
             Description
                Set prestador
        </remarks>
        <typeparam name="TRequest">AgregarPrestadorCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = username
            Denied:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Restorna el nombre de prestador</returns>
    */
    public class AgregarPrestadoresHandler : IRequestHandler<AgregarPrestadorCommand, string>
     {
         private readonly IUCABPagaloTodoDbContext _dbContext;
         private readonly ILogger<AgregarPrestadorCommand> _logger;

         public AgregarPrestadoresHandler(IUCABPagaloTodoDbContext dbContext,ILogger<AgregarPrestadorCommand> logger)
         {
             _dbContext = dbContext;
             _logger = logger;
         }

        /*
        <summary>
            Maneja un request
        </summary>
        <remarks>
             Description
                Handles a request y verifica si el request está lleno
             Parametros
                - TRequest = request de registrar un prestador (AgregarPrestadorCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = Username
            Denied:
                - CustomException
        </response>
        <returns>Restorna el nombre de usuario</returns>    
        */
        public Task<string> Handle(AgregarPrestadorCommand request, CancellationToken cancellationToken)
         {
             try
             {
                 if (request.Request is null)
                 {
                     _logger.LogWarning("AgregarConsumidoresHandler.Handle: Request nulo..");
                     throw new CustomException(new() { "Solicitud Invalida (Agregar Consumidor)" });
                 }
                 else
                 {
                     return HandleAsync(request);
                 }
             }
             catch (Exception ex)
             {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message  }); 
             }
         }
        /*
        <summary>
            Maneja un request ASync
        </summary>
        <remarks>
             Description
                Handles a request, realiza las verificaciones para realizar el Add
             Parametros
                - TRequest = request de registrar un prestador (AgregarPrestadorCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Username
            Denied:
                - CustomException
        </response>
        <returns>Restorna el nombre de usuario</returns>    
        */
        private async Task<string> HandleAsync(AgregarPrestadorCommand request)
         {
            try
             {
                //Se validan con el Validator el formato de los campos
                var validator = new AgregarPrestadorValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    return await AddUserValidation.ValidateAddUser(request.Request, _dbContext);
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
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde", ex.Message  }); 
             }
        }
     }
}
