using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;
using XAct;
using XAct.Users;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class BaseValidation
    {

        /// <summary>
        ///     Validates if the username exist in one of the tables
        /// </summary>
        /// <remarks>
        /// <paramref name="username"/> string that represents the user to be search
        /// <paramref name="_dbContext"/> IUCABPagaloTodoDbContext Context
        /// </remarks>
        /// <returns>Returns the sum of the usernames in the tables</returns>
        /// 
        public static void ValidateUsername(IUCABPagaloTodoDbContext _dbContext, string username)
        {
            var count = _dbContext.Administrador.Count(a => a.Username == username);

            count += _dbContext.Consumidor.Count(c => c.Username == username);

            count += _dbContext.Prestador.Count(p => p.Username == username);

            if (count > 0)
            {
                throw new SQLException(new() { $"El usuario {username} ya existe." });
            }
        }

        /// <summary>
        ///     Validates if the Email exist in one of the tables
        /// </summary>
        /// <remarks>
        ///  <paramref name="correo"/>: string that represents the email to be search
        ///  <paramref name="_dbContext"/> IUCABPagaloTodoDbContext Context
        /// </remarks>
        /// <returns>Returns the sum of the emails in the tables</returns>
        /// 

        public static string ValidateCorreo(IUCABPagaloTodoDbContext _dbContext, string correo)
        {
            var count = _dbContext.Administrador.Count(a => a.Correo == correo);

            count += _dbContext.Consumidor.Count(a => a.Correo == correo);

            count += _dbContext.Prestador.Count(p => p.Correo == correo);

            if (count > 0)
            {
                throw new SQLException(new() { $"El correo {correo} ya existe." });
            }
            return correo;

        }
    }
}
