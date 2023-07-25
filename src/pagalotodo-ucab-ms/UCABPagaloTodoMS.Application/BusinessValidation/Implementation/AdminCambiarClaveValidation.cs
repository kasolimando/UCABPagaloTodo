using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class AdminCambiarClaveValidation : ICambiarClaveUser
    {
        /// <summary>
        ///     Validates the Business Rules To change the user password
        /// </summary>
        /// <remarks>
        /// <paramref name="Request"/> CambioClaveUserRequest with the new status
        ///  <paramref name="_dbContext"/> Context
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 
        public async Task<string> ValidateCambioClaveUser(CambioClaveUserCommand request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var infoAdminActual = await AdminValidation.GetUserById(request.Username, _dbContext);
            var claveActualEncriptada = Encriptacion.EncriptarClave(request.Request.Clave_actual);
            if (infoAdminActual.Clave == claveActualEncriptada)
            {
                infoAdminActual.Clave = Encriptacion.EncriptarClave(request.Request.Clave_nueva);
                _dbContext.Administrador.Update(infoAdminActual);
                await _dbContext.SaveEfContextChanges(infoAdminActual.Username);
                transaction.Commit();
                return infoAdminActual.Username;
            }
            else
            {
                transaction?.Rollback();
                throw new CustomException(new() { "La clave actual no coincide con la registrada" });
            }
        }
    }
}
