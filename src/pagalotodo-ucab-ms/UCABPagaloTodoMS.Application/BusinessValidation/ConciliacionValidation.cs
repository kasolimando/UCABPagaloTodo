using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using XAct.Library.Settings;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class ConciliacionValidation
    {

        /// <summary>
        ///     Validates the Business Rules procesar la conciliacion
        /// </summary>
        /// <remarks>
        /// <paramref name="request"/> GuardarConciliacionRequest
        /// <paramref name="_dbContext"/> Context
        /// </remarks>
        public static async Task<Guid> ProcesarConciliacion(IUCABPagaloTodoDbContext _dbContext, GuardarConciliacionRequest request)
        {
            var transaccion = _dbContext.BeginTransaction();
            //Get the ID of the services name
            var servicio = await ServicioValidation.GetServicio(request.Servicio, _dbContext);
            //Find the pago made
            var Pago = await _dbContext.Pago.Where(p => p.Id == new Guid(request.PagoId)).FirstOrDefaultAsync();
            //The pago has to exist and the prestador should have accepted the pago
            if (Pago is not null && request.Aceptado.Equals("1"))
            {
                //Check if the servicio is type confirmacion o contado
                if (servicio.TipoPago.ToLower().Equals("confirmacion"))
                {
                    await ConciliacionValidation.PagoxConfirmacion(_dbContext, request, Pago);
                }
                else
                {
                    await ConciliacionValidation.PagoAceptado(_dbContext, Pago);
                }
                transaccion.Commit();
            }
            else
            {
                transaccion?.Rollback();
            }
            return Guid.NewGuid();
        }

        /// <summary>
        ///     Validates the Business Rules procesar la conciliacion
        /// </summary>
        /// <remarks>
        /// <paramref name="request"/> GuardarConciliacionRequest
        /// <paramref name="_dbContext"/> Context
        /// <paramref name="_pago"/> PagoEntity
        /// </remarks>
        public static async Task PagoxConfirmacion(IUCABPagaloTodoDbContext _dbContext, GuardarConciliacionRequest request, PagoEntity _pago)
        {
            //Find the dueda of the pago
            var Deuda = await _dbContext.Deuda.Where(d => d.Username == _pago.ConsumidorEntityId && d.servicioId == _pago.ServicioEntityId).FirstOrDefaultAsync();
            //if the deuda is the same monto of the deuda the deuda is remove 
            if(Deuda.Monto == _pago.Monto)
            {
                _dbContext.Deuda.Remove(Deuda);
                await _dbContext.SaveEfContextChanges(Deuda.Username);
            }
            //if the deuda is less than the pago
            if (Deuda.Monto > _pago.Monto)
            {
                //subtracted the amounts
                Deuda.Monto = Deuda.Monto - _pago.Monto;
                //the status is change to activo
                Deuda.Estatus = DeudaEntity.Status.Activo;
                //update the deuda with de new amount
                _dbContext.Deuda.Update(Deuda);
                await _dbContext.SaveEfContextChanges(Deuda.Username);
            }
            //Update the pago to acepted
            await PagoAceptado(_dbContext, _pago);
        }

        /// <summary>
        ///     Validates the Business Rules procesar la conciliacion
        /// </summary>
        /// <remarks>
        /// <paramref name="_dbContext"/> Context
        /// <paramref name="_pago"/> PagoEntity
        /// </remarks>
        public static async Task PagoAceptado(IUCABPagaloTodoDbContext _dbContext, PagoEntity _pago)
        {
            //Change the status of the pago
            _pago.Aprobado = true;
            _dbContext.Pago.Update(_pago);
            await _dbContext.SaveEfContextChanges(_pago.ConsumidorEntityId);
        }

    }
}
