using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class ConsumidorCambiarClaveValidation : ICambiarClaveUser
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

        public async Task<string> ValidateCambioClaveUser(CambioClaveUserCommand request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var infoConsumidorActual = await ConsumidorValidation.GetUserById(request.Username, _dbContext);
            var claveActualEncriptada = Encriptacion.EncriptarClave(request.Request.Clave_actual);
            if (infoConsumidorActual.Clave == claveActualEncriptada)
            {
                infoConsumidorActual.Clave = Encriptacion.EncriptarClave(request.Request.Clave_nueva);
                _dbContext.Consumidor.Update(infoConsumidorActual);
                await _dbContext.SaveEfContextChanges(infoConsumidorActual.Username);
                transaction.Commit();
                return infoConsumidorActual.Username;
            }
            else
            {
                transaction?.Rollback();
                throw new CustomException(new() { "La clave actual no coincide con la registrada" });
            }
        }
    }
}
