
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.BusinessValidation.Interfaces;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.BusinessValidation
{
    public class LoginValidation
    {
        private readonly Dictionary<Type, Func<LoginUsuariosQuery, Task<UsuarioEntity>>> usuarios;
        private readonly IUCABPagaloTodoDbContext _dbContext;

        public LoginValidation(IUCABPagaloTodoDbContext dbContext)
        {
            _dbContext = dbContext;
            usuarios = new Dictionary<Type, Func<LoginUsuariosQuery, Task<UsuarioEntity>>>
            {
                { typeof(PrestadorEntity), BuscarPrestador},
                { typeof(ConsumidorEntity), BuscarConsumidor},
                { typeof(Administrador), BuscarAdministrador}
            };
        }

        public async Task<UsuarioEntity> ValidateCredentials(LoginUsuariosQuery user)
        {
            foreach (var usuario in usuarios.Values)
            {
                var usuarioResponse = await usuario(user);
                if (usuarioResponse != null)
                {
                    return usuarioResponse;
                }
            }
            throw new SQLException(new() {"Las Credenciales no son validas verifique y vuelva a intentarlo"});
        }
        private async Task<UsuarioEntity> BuscarPrestador(LoginUsuariosQuery user)
        {
            return  await _dbContext.Prestador.Where(p => p.Username == user.username && p.Clave == user.clave && p.Estatus == true).FirstOrDefaultAsync();
        }

        private async Task<UsuarioEntity> BuscarConsumidor(LoginUsuariosQuery user)
        {
            return await _dbContext.Consumidor.Where(c => c.Username == user.username && c.Clave == user.clave && c.Estatus == true).FirstOrDefaultAsync();
        }
        private async Task<UsuarioEntity> BuscarAdministrador(LoginUsuariosQuery user)
        {
            return await _dbContext.Administrador.Where(a => a.Username == user.username && a.Clave == user.clave && a.Estatus == true).FirstOrDefaultAsync();
        }

    }
}
