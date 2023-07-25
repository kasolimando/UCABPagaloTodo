using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class DeudasValidation
    {
        public static void HaveDeudasPendientesConsumidor(IUCABPagaloTodoDbContext _dbContext, String username)
        {
            var DeudasPen = _dbContext.Deuda.Any(a => a.Username == username);
            if (DeudasPen)
            {
                throw new SQLException(new() { "El usuario tiene deudas, no se puede desactivar la cuenta" });
            }
        }


        /// <summary>
        ///     Validates the Business Rules To storage a dueda
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - Request: GuardarDeudaRequest
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns the username of the user added</returns>
        /// 
        public static async Task<Guid> GuardarDeuda(IUCABPagaloTodoDbContext _dbContext, GuardarDeudaRequest request)
        {
            var transaccion = _dbContext.BeginTransaction();
            //Find th ID of the service name
            var servicio = await ServicioValidation.GetServicio(request.Servicio, _dbContext);
            //If the consumidor exist
            if (await _dbContext.Consumidor.Where(p => p.Username == request.Username).FirstOrDefaultAsync() is not null)
            {
                //Find the deudas between the services and the consumidor
                var deuda = await _dbContext.Deuda.Where(d => d.servicioId == servicio.Id && d.Username == request.Username).FirstOrDefaultAsync();
                //if the amount is correct
                if (Double.Parse(request.Monto) > 0)
                {
                    //if the deuda doesn't exist
                    if (deuda is null)
                    {
                        //change the request to a entity
                        var entity = DeudasMapper.MapRequestAEntity(request.Username, servicio.Id, Double.Parse(request.Monto));
                        //create the new deuda
                        _dbContext.Deuda.Add(entity);
                        await _dbContext.SaveEfContextChanges(entity.Username);
                    }
                    else
                    {
                        //if the deuda already exist change the amount
                        deuda.Monto = Double.Parse(request.Monto);
                        //update the deuda
                        _dbContext.Deuda.Update(deuda);
                        await _dbContext.SaveEfContextChanges(deuda.Servicio.Nombre);
                    }
                    transaccion.Commit();
                }
            }
            else
            {
                transaccion?.Rollback();
            }
            return Guid.NewGuid();
        }
    }
}
