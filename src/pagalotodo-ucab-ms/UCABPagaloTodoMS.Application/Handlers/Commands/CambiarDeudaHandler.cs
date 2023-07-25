using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using Microsoft.EntityFrameworkCore;

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
    public class CambiarDeudaHandler : IRequestHandler<CambiarDeudaCommand, string>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CambiarDeudaCommand> _logger;

        public CambiarDeudaHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CambiarDeudaCommand> logger)
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
        
        public Task<string> Handle(CambiarDeudaCommand request, CancellationToken cancellationToken)
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
        private async Task<string> HandleAsync(CambiarDeudaCommand request)
        {
            try
            {
                using var transaction = _dbContext.BeginTransaction();
                var consumidor = await _dbContext.Consumidor.Where(p => p.Username == request.Username).FirstOrDefaultAsync();
                var servicio = await _dbContext.Servicio.Where(p => p.Nombre == request.Servicio).FirstOrDefaultAsync();
                if (consumidor is not null && servicio is not null)
                {
                    var entity = DeudasMapper.MapRequestAEntity(consumidor.Username, servicio.Id, Double.Parse(request.Monto));
                    _dbContext.Deuda.Add(entity);
                    await _dbContext.SaveEfContextChanges(servicio.Nombre);
                    transaction.Commit();
                    return servicio.Nombre;
                }
                else
                {
                    transaction?.Rollback();
                    return "";
                }
            }
            catch (Exception ex) { throw new CustomException(new() { "Disculpe, hubo un error, por favor intente mas tarde ", ex.Message }); }
        }
    }
}

