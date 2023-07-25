using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using XAct;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class PrestadorValidation : BaseValidation
    {

        public static async Task<PrestadorEntity> GetUserById(string username, IUCABPagaloTodoDbContext dbContext)
        {
            var consumidor = await dbContext.Prestador.FirstOrDefaultAsync(a => a.Username == username);
            if (consumidor is null)
            {
                throw new CustomException(new() { $"El usuario {username} no existe." });
            }
            return consumidor;
        }

        /// <summary>
        ///     Validates the Business Rules To enable a user
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actualInfo: PrestadorEntity
        ///         - _dbContext: Context
        /// </remarks>
        /// 
        public static async Task EnableUser(PrestadorEntity actualInfo, IUCABPagaloTodoDbContext dbContext)
        {
            await ActualizarPrestadorEstatus(actualInfo, true, dbContext);
            await ServicioValidation.UpdateServiciosEstado(actualInfo.Username, ServicioEntity.Status.activo, dbContext);
        }

        /// <summary>
        ///    Validate if the username has pending payments
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - username: string
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns a bool</returns>

        private static async Task<bool> HasPendingPayments(string username, IUCABPagaloTodoDbContext dbContext)
        {
            return dbContext.Pago.Join(dbContext.Servicio, pago => pago.ServicioEntityId, servicio => servicio.Id, (pago, servicio) => new { Pago = pago, Servicio = servicio })
                .Any(s => s.Servicio.PrestadorEntityId.Equals(username) && s.Pago.Cierre == false);
        }


        /// <summary>
        ///    Validate if the username has debts payments
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - username: string
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns a bool</returns>
        private static async Task<bool> HasPendingDebts(string username, IUCABPagaloTodoDbContext dbContext)
        {
            return dbContext.Deuda.Join(dbContext.Servicio, deuda => deuda.servicioId, servicio => servicio.Id, (deuda, servicio) => new { Deudas = deuda, Servicio = servicio })
                .Any(s => s.Servicio.PrestadorEntityId.Equals(username));
        }

        /// <summary>
        ///    disble the user
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actualInfo: PrestadorEntity
        ///         - _dbContext: Context
        /// </remarks>
        public static async Task DisableUserIfPossible(PrestadorEntity actualInfo, IUCABPagaloTodoDbContext dbContext)
        {
            if (await HasPendingPayments(actualInfo.Username, dbContext))
            {
                throw new SQLException(new() { "El usuario tiene servicios con pagos que no se han mandado a cierre, no se puede desactivar la cuenta" });
            }

            if (await HasPendingDebts(actualInfo.Username, dbContext))
            {
                throw new SQLException(new() { "El usuario tiene servicios con deudas no se puede desactivar la cuenta" });
            }
            await ActualizarPrestadorEstatus(actualInfo, false, dbContext);
            await ServicioValidation.UpdateServiciosEstado(actualInfo.Username, ServicioEntity.Status.inactivo, dbContext);
        }

        public static async Task ActualizarPrestadorEstatus(PrestadorEntity actualInfo, bool estatus, IUCABPagaloTodoDbContext dbContext)
        {
            actualInfo.Estatus = estatus;
            dbContext.Prestador.Update(actualInfo);
            await dbContext.SaveEfContextChanges(actualInfo.Username);
        }
    }
}

