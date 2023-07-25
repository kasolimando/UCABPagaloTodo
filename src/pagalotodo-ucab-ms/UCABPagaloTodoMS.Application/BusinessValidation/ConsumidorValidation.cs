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
    public class ConsumidorValidation :BaseValidation
    {

        public static async Task<ConsumidorEntity> GetUserById(string username, IUCABPagaloTodoDbContext dbContext)
        {
            var consumidor = await dbContext.Consumidor.FirstOrDefaultAsync(a => a.Username == username);
            if (consumidor is null)
            {
                throw new CustomException(new() { $"El usuario {username} no existe." });
            }
            return consumidor;
        }
    }
}
