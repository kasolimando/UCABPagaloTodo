using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Interfaces
{
    public interface IStatusUserValidator
    {
        Task<string> ValidateStatusUser(StatusUserCommand request, IUCABPagaloTodoDbContext dbContext);
    }
}
