using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Infrastructure.Services.Hotmail;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class RecuperarClaveValidation
    {
        private readonly Dictionary<Type, Func<string, Task<UsuarioEntity>>> usuarios;
        private readonly IUCABPagaloTodoDbContext _dbContext;

        public RecuperarClaveValidation(IUCABPagaloTodoDbContext dbContext)
        {
            _dbContext = dbContext;
            usuarios = new Dictionary<Type, Func<string, Task<UsuarioEntity>>>
            {
                { typeof(PrestadorEntity), BuscarPrestador},
                { typeof(ConsumidorEntity), BuscarConsumidor},
                { typeof(Administrador), BuscarAdministrador}
            };
        }

        public async Task<UsuarioEntity> GetUser(string user)
        {
            foreach (var usuario in usuarios.Values)
            {
                var usuarioResponse = await usuario(user);
                if (usuarioResponse != null)
                {
                    return usuarioResponse;
                }
            }
            throw new SQLException(new() { "El usuario no se encuentra registrado" });
        }
        private async Task<UsuarioEntity> BuscarPrestador(string user)
        {
            using var transaction = _dbContext.BeginTransaction();
            var User = await _dbContext.Prestador.Where(p => p.Username == user).FirstOrDefaultAsync();
            if(User != null)
            {
                var clave = EnviarCorreoClave.EnviarCorreoRecuperacion(User.Correo);
                User.Clave = Encriptacion.EncriptarClave(clave);
                _dbContext.Prestador.Update((PrestadorEntity)User);
                await _dbContext.SaveEfContextChanges(User.Username);
                transaction.Commit();
            }
            return User;
        }

        private async Task<UsuarioEntity> BuscarConsumidor(string user)
        {
            using var transaction = _dbContext.BeginTransaction();
            var User = await _dbContext.Consumidor.Where(c => c.Username == user).FirstOrDefaultAsync();
            if (User != null)
            {
                var clave = EnviarCorreoClave.EnviarCorreoRecuperacion(User.Correo);
                User.Clave = Encriptacion.EncriptarClave(clave);
                _dbContext.Consumidor.Update((ConsumidorEntity)User);
                await _dbContext.SaveEfContextChanges(User.Username);
                transaction.Commit();
            }
            return User;
        }
        private async Task<UsuarioEntity> BuscarAdministrador(string user)
        {
            using var transaction = _dbContext.BeginTransaction();
            var User = await _dbContext.Administrador.Where(a => a.Username == user).FirstOrDefaultAsync();
            if (User != null)
            {
                var clave = EnviarCorreoClave.EnviarCorreoRecuperacion(User.Correo);
                User.Clave = Encriptacion.EncriptarClave(clave);
                _dbContext.Administrador.Update((Administrador)User);
                await _dbContext.SaveEfContextChanges(User.Username);
                transaction.Commit();
            }
            return User;
        }

    }
}
