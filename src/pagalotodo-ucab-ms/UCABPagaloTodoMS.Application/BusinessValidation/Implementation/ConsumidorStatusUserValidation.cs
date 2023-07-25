using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class ConsumidorStatusUserValidation : IStatusUserValidator
    {
        /// <summary>
        ///     Validates the Business Rules To Update a Status Consumidor
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - Request: ConsumidorRequest with the information of the Update Request
        ///         - _dbContext: Context
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 

        public async Task<string> ValidateStatusUser(StatusUserCommand request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var Consumidor = await ConsumidorValidation.GetUserById(request.Username, _dbContext);
            if (Consumidor.Estatus)
            {
                PagoValidation.HavePagosPendientesConsumidor(_dbContext, request.Username);
                DeudasValidation.HaveDeudasPendientesConsumidor(_dbContext, request.Username);
            }
            Consumidor.Estatus = request.Request.Estatus;
            _dbContext.Consumidor.Update(Consumidor);
            await _dbContext.SaveEfContextChanges(Consumidor.Username);
            transaction.Commit();
            return Consumidor.Username;
        }
    }
}
