using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class PagoValidation
    {

        public static void HavePagosPendientesConsumidor(IUCABPagaloTodoDbContext _dbContext, String username)
        {
            var PagosPen = _dbContext.Pago.Any(a => a.ConsumidorEntityId == username && a.Cierre == false);
            if (PagosPen)
            {
                throw new SQLException(new() { "El usuario tiene pagos que no se han mandado a cierre, no se puede desactivar la cuenta" });
            }
        }

        /// <summary>
        ///     Validates the Business Rules To create a Pago
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - _result: ValidationResult that contains the result of the Validator
        ///         - Request: PagoRequest with the informatioon of the new Pago
        ///         - _dbContext: Context
        ///         - ExceptionValidator: ValidatorException the custom validation of the validators
        /// </remarks>
        /// <returns>Returns the name of pago's servicio created</returns>
        /// 

        public async Task<string> ValidateAddPago(ValidationResult _result, PagoRequest request, IUCABPagaloTodoDbContext _dbContext, ValidatorException ExceptionValidator)
        {
            using var transaction = _dbContext.BeginTransaction();
            if (_result.IsValid)
            {
                //The Request.Servicio is the name, so this get the Servicio's Guid
                var servicio = await ServicioValidation.GetServicio(request.Servicio, _dbContext);
                var consumidor = await _dbContext.Consumidor.Where(p => p.Username == request.Consumidor).FirstOrDefaultAsync();
                if(consumidor is null)
                {
                    transaction?.Rollback();
                    throw new CustomException(new() { "El consumidor que esta indicando no existe"});
                }
                else
                {
                    if (servicio.Estatus == Status.activo)
                    {
                        if (servicio.TipoPago.ToLower().Equals("confirmacion"))
                        {
                            var resultServicio = await AddPagoConfirmacion(request, _dbContext, servicio, consumidor);
                            transaction.Commit();
                            return servicio.Nombre;

                        }
                        else
                        {
                            var resultServicio = await AddPagoContado(request, _dbContext, servicio, consumidor);
                            transaction.Commit();
                            return servicio.Nombre;
                        }
                    }
                    else
                    {
                        transaction?.Rollback();
                        throw new CustomException(new() { "El servicio debe estar activo para poder realizar pagos" });
                    }
                }
            }
            else
            {
                transaction?.Rollback();
                throw ExceptionValidator;
            }
        }
        /// <summary>
        ///     Validates the Business Rules To create a Pago por confirmacion, int his pago the consumidor must have a deuda
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - Request: PagoRequest with the information of the new Pago
        ///         - _dbContext: Context
        ///         - Servicio: ServicioEntity the servicio of the new Pago
        ///         - consumidor : ConsumidorEntity of the new Pago
        /// </remarks>
        /// <returns>Returns the name of pago's servicio created</returns>
        /// 
        public async Task<string> AddPagoConfirmacion(PagoRequest request, IUCABPagaloTodoDbContext _dbContext, ServicioEntity Servicio, ConsumidorEntity consumidor)
        {

            var deuda = await _dbContext.Deuda.Where(d => d.servicioId == Servicio.Id && d.Username == consumidor.Username && d.Estatus == DeudaEntity.Status.Activo).FirstOrDefaultAsync();
            if (deuda is not null)
            {
                var entity = PagosMapper.MapRequestEntityWithADeuda(request, Servicio.Id, false, false, deuda.Monto);
                _dbContext.Pago.Add(entity);
                await _dbContext.SaveEfContextChanges(Servicio.Nombre);
                deuda.Estatus = DeudaEntity.Status.Inactivo;
                _dbContext.Deuda.Update(deuda);
                await _dbContext.SaveEfContextChanges(deuda.Servicio.Nombre);
                return Servicio.Nombre;
            }
            else
            {
                throw new CustomException(new() { "El consumidor indicado no tiene una deuda activa con el servicio" });
            }
        }
        /// <summary>
        ///     Validates the Business Rules To create a Pago por contado 
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - Request: PagoRequest with the information of the new Pago
        ///         - _dbContext: Context
        ///         - Servicio: ServicioEntity the servicio of the new Pago
        ///         - consumidor : ConsumidorEntity of the new Pago
        /// </remarks>
        /// <returns>Returns the name of pago's servicio created</returns>
        /// 
        public async Task<string> AddPagoContado(PagoRequest request, IUCABPagaloTodoDbContext _dbContext, ServicioEntity Servicio, ConsumidorEntity consumidor)
        {
            if (request.Monto > 0)
            {
                var entity = PagosMapper.MapRequestEntityWithADeuda(request, Servicio.Id, false, false, request.Monto);
                _dbContext.Pago.Add(entity);
                await _dbContext.SaveEfContextChanges(Servicio.Nombre);
                return Servicio.Nombre;
            }
            else
            {
                throw new CustomException(new() { "El monto debe ser mayor que 0" });
            }
        }

        /// <summary>
        ///     Validates the Business Rules make a cierre
        /// </summary>
        /// <remarks>
        /// <paramref name="ListaPagos"/> List<PagoEntity> with the pagos
        /// <paramref name="_dbContext"/> Context
        /// <paramref name="Prestador"/> PrestadorEntity the prestador object
        /// </remarks>

        public static async Task ActualizarCierrePago(List<PagoEntity> ListaPagos, IUCABPagaloTodoDbContext _dbContext, PrestadorEntity Prestador, DateTime FechaConci)
        {
            //In eachone of the pagos update the cierre
            foreach (var pago in ListaPagos)
            {
                pago.Cierre = true;
                pago.FechaCierre = FechaConci;
                _dbContext.Pago.Update(pago);
                await _dbContext.SaveEfContextChanges(Prestador.Username);
            }
        }
    }
}
