using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.BusinessValidation;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{

    /*
        <summary>
          Handler the AgregarPagoCommand
        </summary>
        <remarks>
             Description
                POST Deudas
        </remarks>
        <typeparam name="TRequest">AgregarPagoCommand</typeparam>
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
    public class AgregarPagoHandler : IRequestHandler<AgregarPagoCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarPagoCommand> _logger;

        public AgregarPagoHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarPagoCommand> logger)
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
                - TRequest = Request to add a Pago (AgregarPagoCommand)
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

        public Task<string> Handle(AgregarPagoCommand request, CancellationToken cancellationToken)
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
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message  });
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
                - TRequest = request to add a Pago (AgregarPagoCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> =  Servicio's name
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's name</returns>    
        */
        private async Task<string> HandleAsync(AgregarPagoCommand request)
        {
            try
            {
                //Se validan con el Validator el formato de los campos
                var validator = new AgregarPagoValidator();
                ValidationResult result = validator.Validate(request);
                //Se valida la existencia para luego realizar el Add
                var _pagoValidation = new PagoValidation();
                //new(result) crea la excepcion de tipo ValidatorException con la informacion del validator
                return await _pagoValidation.ValidateAddPago(result, request.Request, _dbContext, new(result));
            }
            catch (ValidatorException ex)
            {
                throw new CustomException(ex.GetMessages(), ex.GetCodes());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message  });
            }
        }
    }
}
