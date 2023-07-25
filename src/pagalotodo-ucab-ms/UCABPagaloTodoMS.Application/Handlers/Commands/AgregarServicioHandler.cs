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
          Handler the AgregarServicioCommand
        </summary>
        <remarks>
             Description
                POST Formatos
        </remarks>
        <typeparam name="TRequest">AgregarServicioCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = Servicio's name
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Return the Servicio's name</returns>
    */
    public class AgregarServicioHandler : IRequestHandler<AgregarServicioCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarServicioCommand> _logger;

        public AgregarServicioHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarServicioCommand> logger)
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
                - TRequest = Request to add a Servicio (AgregarServicioCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = Servicio's name
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's name</returns>    
        */
        public Task<string> Handle(AgregarServicioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Request is null)
                {
                    throw new CustomException(new() { "Solicitud Invalida" });
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
                - TRequest = request to add a Servicio (AgregarServicioCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Servicio' name
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's name</returns>    
        */
        private async Task<string> HandleAsync(AgregarServicioCommand request)
        {
            try
            {
                //Se validan con el Validator el formato de los campos
                var validator = new ServicioValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    return await ServicioValidation.ValidateAddServicio(request.Request, _dbContext);
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
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }
    }
}

