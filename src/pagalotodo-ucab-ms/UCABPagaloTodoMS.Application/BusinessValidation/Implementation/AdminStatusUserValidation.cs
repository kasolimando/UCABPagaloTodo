using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class AdminStatusUserValidation : IStatusUserValidator
    {
        /// <summary>
        ///     Validates the Business Rules To change the user's status
        /// </summary>
        /// <remarks>
        ///  <paramref name="Request"/> StatusUserRequest with the new status
        ///  <paramref name="_dbContext"/>: Context
        /// </remarks>
        /// <returns>Returns the username of the user updated</returns>
        /// 

        public async Task<string> ValidateStatusUser(StatusUserCommand request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var ActualInfo = await _dbContext.Administrador.Where(a => a.Username == request.Username).FirstOrDefaultAsync();
            if (ActualInfo is null)
            {
                transaction?.Rollback();
                throw new SQLException(new() { "El usuario no se encuentra registrado" });
            }
            else
            {
                ActualInfo.Estatus = request.Request.Estatus;
                _dbContext.Administrador.Update(ActualInfo);
                await _dbContext.SaveEfContextChanges(ActualInfo.Username);
                transaction.Commit();
                return ActualInfo.Username;
            }
        }
    }
}
