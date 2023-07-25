using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class ServicioValidation
    {

        public static async Task UpdateServiciosEstado(string prestadorId, ServicioEntity.Status estado, IUCABPagaloTodoDbContext dbContext)
        {
            var servicios = dbContext.Servicio.Where(s => s.PrestadorEntityId == prestadorId);
            foreach (var servicio in servicios)
            {
                servicio.Estatus = estado;
            }
            dbContext.Servicio.UpdateRange(servicios);
            await dbContext.SaveEfContextChanges(prestadorId);
        }


        /// <summary>
        ///     Validates the Business Rules To create a Servicio
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: ServicioRequest with the informatioon of the new Servicio
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the name of the service added</returns>
        /// 
        public static async Task<string> ValidateAddServicio(ServicioRequest request, IUCABPagaloTodoDbContext dbContext)
        {
            using var transaction = dbContext.BeginTransaction();
            var prestador = await PrestadorValidation.GetUserById(request.PrestadorEntityId, dbContext);
            if (prestador.Estatus == true){
                await ValidateServicio(request.Nombre, dbContext);
                var requestAsServicio = ServiciosMapper.MapRequestEntity(request);
                dbContext.Servicio.Add(requestAsServicio);
                await dbContext.SaveEfContextChanges(requestAsServicio.Nombre);
                transaction.Commit();
                return requestAsServicio.Nombre;
            } 
            else{
                transaction?.Rollback();
                throw new SQLException(new() { "El prestador no se encuentra activo" });
            }
        }

        /// <summary>
        ///     Search The Guid of a service's name
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _nombre: string with the service's name
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns the Guid of the service</returns>
        /// 
        public static async Task<ServicioEntity> GetServicio(string _nombre, IUCABPagaloTodoDbContext _dbContext)
        {
            var ActualInfo = await _dbContext.Servicio.FirstOrDefaultAsync(s => s.Nombre == _nombre);
            if (ActualInfo is null)
            {
                throw new SQLException(new() { "El servicio no se encuentra registrado" });
            }
            else
            {
                return ActualInfo;
            }
        }

        /// <summary>
        ///     Search The Guid of a service's name
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _nombre: string with the service's name
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns the Guid of the service</returns>
        /// 
        public static async Task ValidateServicio(string _nombre, IUCABPagaloTodoDbContext _dbContext)
        {
            var ActualInfo = await _dbContext.Servicio.FirstOrDefaultAsync(s => s.Nombre == _nombre);
            if (ActualInfo is not null)
            {
                throw new SQLException(new() { "El servicio ya se encuentra registrado" });
            }
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

        private static async Task HasPendingPayments(Guid servicio, IUCABPagaloTodoDbContext dbContext)
        {
            var PagosPen = dbContext.Pago.Any(a => a.ServicioEntityId == servicio && a.Cierre == false);
            if (PagosPen)
            {
                throw new SQLException(new() { "El servicio tiene pagos que no se han mandado a cierre, no se puede desactivar la cuenta" });
            }
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
        private static async Task HasPendingDebts(Guid servicio, IUCABPagaloTodoDbContext dbContext)
        {
            var deuda = dbContext.Deuda.Any(a => a.servicioId == servicio);
            if (deuda)
            {
                throw new SQLException(new() { "El servicio tiene deudas, no se puede desactivar la cuenta" });
            }
        }

        /// <summary>
        ///     Validates the Business Rules To update a servicio's formato
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _Servicio: The servicio's Guid to be updated
        ///         - Guid:  The formatos's Guid 
        ///         - _dbContext: Context
        /// </remarks>
        /// 
        public static async Task AgregarFormatoServicio(IUCABPagaloTodoDbContext _dbContext,Guid _Servicio, Guid _Formato)
        {
            try
            {
                var ActualServicio = await _dbContext.Servicio.Where(s => s.Id == _Servicio).FirstOrDefaultAsync();
                ActualServicio.FormatoConEntityId = _Formato;
                _dbContext.Servicio.Update(ActualServicio);
                await _dbContext.SaveEfContextChanges(ActualServicio.Nombre);

            }catch (Exception){
                throw new CustomException(new () { "Disculpe ha ocurrido un error al agregar el formato al servicio" });
            }
        }

        public static async Task<List<PagoEntity>> GetPagosPendientesCierre(ServicioEntity servicio, IUCABPagaloTodoDbContext dbContext)
        {
            var ListaPagos = await dbContext.Pago.Include(p => p.Consumidor).Where(p => p.ServicioEntityId == servicio.Id && p.Cierre == false).ToListAsync();
            //If ListaPagos > 0 the service has payments in false 
            if (ListaPagos.Count() == 0)
                throw new CustomException(new() { "El servicio no cuenta con pagos pendientes por cerrar" });
            return ListaPagos;
        }

        /// <summary>
        ///     Validates the Business Rules To change the user's status
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _servicio: string that represents the servicio to be Updated
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: ServicioRequest with the new info
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the servicio's name updated</returns>
        /// 
        public static async Task<string> ValidateUpdateServicio(ServicioRequest request, IUCABPagaloTodoDbContext dbContext)
        {
            using var transaction = dbContext.BeginTransaction();
            var servicio = await GetServicio(request.Nombre, dbContext);
            var requestAsServicio = ServiciosMapper.MapRequestEntity(request);
            servicio = ServiciosMapper.MapRequestUpdateEntitySinStatus(servicio, requestAsServicio);
            dbContext.Servicio.Update(servicio);
            await dbContext.SaveEfContextChanges(servicio.Nombre);
            transaction.Commit();
            return servicio.Nombre;
        }

        // Validate statusServicio
        public static async Task<string> ValidateStatusServicio(StatusServiciosCommand request, IUCABPagaloTodoDbContext dbContext)
        {
            using var transaction = dbContext.BeginTransaction();
            var servicio = await GetServicio(request.Servicio, dbContext);
            if (request.Request.Estatus.Equals("activo"))
            {
                servicio.Estatus = ServicioEntity.Status.activo;
                dbContext.Servicio.Update(servicio);
                await dbContext.SaveEfContextChanges(servicio.Nombre);
                transaction.Commit();
                return servicio.Nombre;
            }
            else
            {
                HasPendingPayments(servicio.Id, dbContext);
                HasPendingDebts(servicio.Id, dbContext);
                servicio.Estatus = Enum.Parse<Status>(request.Request.Estatus.ToLower());
                dbContext.Servicio.Update(servicio);
                await dbContext.SaveEfContextChanges(servicio.Nombre);
                transaction.Commit();
                return servicio.Nombre;
            }
        }

    }
}
