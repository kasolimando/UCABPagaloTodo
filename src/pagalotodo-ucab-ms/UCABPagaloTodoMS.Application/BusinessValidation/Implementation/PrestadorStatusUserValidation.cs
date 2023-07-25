using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class PrestadorStatusUserValidation : IStatusUserValidator
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
            var ActualInfo = await PrestadorValidation.GetUserById(request.Username, _dbContext);
            if (request.Request.Estatus)
            {
                await PrestadorValidation.EnableUser(ActualInfo, _dbContext);
            }
            else
            {
                await PrestadorValidation.DisableUserIfPossible(ActualInfo, _dbContext);
            }
            transaction.Commit();
            return ActualInfo.Username;
        }
    }
}
