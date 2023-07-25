﻿using UCABPagaloTodoMS.Application.Commands;
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
          Handler the AgregarFormatoCommand
        </summary>
        <remarks>
             Description
                POST Formatos
        </remarks>
        <typeparam name="TRequest">AgregarFormatoCommand</typeparam>
        <typeparam name="TResponse">string</typeparam>
        <response>
            Response for a request
            Accepted:
                - Task<string> = Servicio's formato name
            Failed:
                - Error en formato de campos = ValidatorException
                - Error de data = SQLException
                - Resto de errores = Exception
                Retorna los tres errores en el tipo CustomException
        </response>
        <returns>Returns the Servicio's formato name</returns>
    */
    public class AgregarFormatoHandler : IRequestHandler<AgregarFormatoCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarFormatoCommand> _logger;

        public AgregarFormatoHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarFormatoCommand> logger)
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
                - TRequest = Request to add a Format (AgregarFormatoCommand)
                - CancellationToken 
        </remarks>
        <response>
            Accepted:
                - Task<string> = Servicio
            Failed:
                - CustomException
        </response>
        <returns>Returns the Servicio's formato name</returns>    
        */
        public Task<string> Handle(AgregarFormatoCommand request, CancellationToken cancellationToken)
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
                - TRequest = request to add a Format (AgregarFormatoCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> = Servicio
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's formato name</returns>    
        */
        private async Task<string> HandleAsync(AgregarFormatoCommand request)
        {
            try
            {
                //A instance to check the info's format with the Validator
                var validator = new FormatoValidator();
                //Check the info's format and is stored on result
                ValidationResult result = validator.Validate(request);
                if (result.IsValid)
                {
                    return await FormatoValidation.ValidateFormatoAdd(request.Request, _dbContext);
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

