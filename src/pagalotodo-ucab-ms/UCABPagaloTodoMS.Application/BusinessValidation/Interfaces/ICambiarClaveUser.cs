using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Interfaces
{
    public interface ICambiarClaveUser
    {
        Task<string> ValidateCambioClaveUser(CambioClaveUserCommand request, IUCABPagaloTodoDbContext dbContext);
    }
}
