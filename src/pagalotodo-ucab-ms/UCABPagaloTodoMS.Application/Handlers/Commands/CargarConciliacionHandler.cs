using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Infrastructure.Services.RabbitProducer;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /*
        <summary>
          Handler the CargarConciliacionCommand
        </summary>
        <remarks>
             Description
                POST Deudas
        </remarks>
        <typeparam name="TRequest">CargarConciliacionCommand</typeparam>
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
    public class CargarConciliacionHandler : IRequestHandler<CargarConciliacionCommand, string>
    {
        private readonly ILogger<CargarConciliacionCommand> _logger;
        private readonly IRabbitProducerConciliacion _producer;

        public CargarConciliacionHandler(ILogger<CargarConciliacionCommand> logger,
            IRabbitProducerConciliacion producer)
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
                - TRequest = Request to cargar conciliacion (CargarConciliacionCommand)
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

        public Task<string> Handle(CargarConciliacionCommand request, CancellationToken cancellationToken)
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
                - TRequest = request to cargar a conciliacion (CargarConciliacionCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> =  Consumidor's username
            Denied:
                - CustomException
        </response>
        <returns>Returns the Consumidor's username</returns>    
        */
        private async Task<string> HandleAsync(CargarConciliacionCommand request)
        {
            try
            {
                _logger.LogInformation("CargarConciliacionHandler");
                await _producer.SendProductMessageConciliacion(request.Archivo);
                return request.Archivo.FileName;
            }
            catch (Exception ex)
            {
                throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message });
            }
        }
    }
}

