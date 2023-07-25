using FluentValidation.Results;
using MediatR;
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
         Handler the CierreCommand
       </summary>
       <remarks>
            Description
               POST Deudas
       </remarks>
       <typeparam name="TRequest">CierreCommand</typeparam>
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
    public class CierreHadler : IRequestHandler<CierreCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CierreCommand> _logger;

        public CierreHadler(IUCABPagaloTodoDbContext dbContext, ILogger<CierreCommand> logger)
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
                - TRequest = Request to make a Cierre (CierreCommand)
        </remarks>
        <response>
            Accepted:
                - Task<string> = Servicio's name
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's name</returns>    
        */

        public Task<string> Handle(CierreCommand request, CancellationToken cancellationToken)
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
                - TRequest = request to make a Cierre(CierreCommand)
        </remarks>
        <response>
            Accepted:
                - async Task<string> =  Servicio's name
            Denied:
                - CustomException
        </response>
        <returns>Returns the Servicio's name</returns>    
        */
        private async Task<string> HandleAsync(CierreCommand request)
        {
            try
            {
                await CierreValidation.MakeCierre(request.Servicio, _dbContext);
                return "ok";
            }
            catch (SQLException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.GetErrorMessage());
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }
    }
}
