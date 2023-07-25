using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /*
        <summary>
          Handler the AgregarDeudaCommand
        </summary>
        <remarks>
             Description
                POST Deudas
        </remarks>
        <typeparam name="TRequest">AgregarDeudaCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = Consumidor's username
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Return the Consumidor's username</returns>
    */
    public class AgregarDeudaHandler : IRequestHandler<AgregarDeudaCommand, string>
    {
        private readonly ILogger<AgregarDeudaCommand> _logger;
        private readonly IRabbitProducer _producer;

        public AgregarDeudaHandler(ILogger<AgregarDeudaCommand> logger,IRabbitProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        /*
        <summary>
            Handle the Request
        </summary>
        <remarks>
             Description
                Handles a Request and check if the request is null
             Parametros
                - TRequest = Request to add a Deuda (AgregarDeudaCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = Consumidor's username
            Denied:
                - CustomException
        </response>
        <returns>Returns the Consumidor's username</returns>    
        */
        
        public Task<string> Handle(AgregarDeudaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
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
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message });
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
                - TRequest = request to add a Deuda (AgregarDeudaCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> =  Consumidor's username
            Denied:
                - CustomException
        </response>
        <returns>Returns the Consumidor's username</returns>    
        */
        private async Task<string> HandleAsync(AgregarDeudaCommand request)
        {
            try
            {
                _logger.LogInformation("AgregarDeudaHandler");
                //Se validan con el Validator el formato de los campos
                var validator = new DeudaValidator();
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    //Se llama al productor de rabbit para que suba el archvio a la cola
                    await _producer.SendProductMessage(request.Archivo);
                    return request.Archivo.FileName;
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
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message });
            }
        }
    }
}

