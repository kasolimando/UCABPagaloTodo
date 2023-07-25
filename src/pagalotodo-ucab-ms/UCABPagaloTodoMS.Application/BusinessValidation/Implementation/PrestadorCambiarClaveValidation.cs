using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.BusinessValidation.Implementation
{
    public class PrestadorCambiarClaveValidation : ICambiarClaveUser
    {
        public async Task<string> ValidateCambioClaveUser(CambioClaveUserCommand request, IUCABPagaloTodoDbContext _dbContext)
        {
            using var transaction = _dbContext.BeginTransaction();
            var infoPrestadorActual = await PrestadorValidation.GetUserById(request.Username, _dbContext);
            var claveActualEncriptada = Encriptacion.EncriptarClave(request.Request.Clave_actual);
            if (infoPrestadorActual.Clave == claveActualEncriptada)
            {
                infoPrestadorActual.Clave = Encriptacion.EncriptarClave(request.Request.Clave_nueva);
                _dbContext.Prestador.Update(infoPrestadorActual);
                await _dbContext.SaveEfContextChanges(infoPrestadorActual.Username);
                transaction.Commit();
                return infoPrestadorActual.Username;
            }
            else
            {
                transaction?.Rollback();
                throw new CustomException(new() { "La clave actual no coincide con la registrada" });
            }
        }
    }
}
