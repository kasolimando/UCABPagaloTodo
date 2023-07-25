using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using XAct;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class AdminValidation: BaseValidation
    {
        public static async Task<Administrador> GetUserById(string username, IUCABPagaloTodoDbContext dbContext)
        {
            var admin = await dbContext.Administrador.FirstOrDefaultAsync(a => a.Username == username);
            if (admin is null)
            {
                throw new CustomException(new() { $"El usuario {username} no existe." });
            }
            return admin;
        }
    }
}
