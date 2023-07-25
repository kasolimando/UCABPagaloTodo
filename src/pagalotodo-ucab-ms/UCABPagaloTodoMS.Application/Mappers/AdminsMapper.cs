using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class AdminsMapper
    {
        public static AdminsResponse MapEntityAResponse(Administrador entity)
        {
            var response = new AdminsResponse()
            {
                Username = entity.Username,
                Correo = entity.Correo,
                Nombre = entity.Nombre +" "+ entity.Apellido,
                Direccion = entity.Direccion,
                DocIdentidad = entity.TipoVj+"-"+entity.DocIdentidad
            };
            return response;
        }


        /// <summary>
        ///     Change a AdminsRequest to a Administrador entity
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: AdminsRequest with the information to register
        /// </remarks>
        /// <returns>Returns the adminsitrador with the new data</returns>
        /// 

        public static Administrador MapRequestEntity(AdminsRequest request, string username)
        {

            var entity = new Administrador()
            {
                Username = username,
                Clave = Encriptacion.EncriptarClave(request.Clave),
                Correo = request.Correo,
                DocIdentidad = request.DocIdentidad,
                TipoVj = request.TipoVj,
                Direccion = request.Direccion ?? string.Empty,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Estatus = true
            };
            return entity;
        }


        /// <summary>
        ///     Switch the New values with the old values
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actual: The administrador to be update
        ///         - entity: The administrador with the new information
        /// </remarks>
        /// <returns>Returns the adminsitrador with the new data</returns>
        /// 
        public static Administrador MapRequestUpdateEntity(Administrador actual, Administrador entity)
        {
            actual.DocIdentidad = entity.DocIdentidad;
            actual.TipoVj = entity.TipoVj;
            actual.Correo = entity.Correo;
            actual.Direccion = entity.Direccion;
            actual.Nombre = entity.Nombre;
            actual.Apellido = entity.Apellido;
            return actual;
        }

        public static Administrador MapUserAEntity(UsuarioEntity entity)
        {
            var response = new Administrador()
            {
                Username = entity.Username,
                Clave = entity.Clave,
                Correo = entity.Correo,
                Nombre = entity.Nombre,
                TipoVj = entity.TipoVj,
                Apellido = entity.Apellido,
                DocIdentidad = entity.DocIdentidad,
                Direccion = entity.Direccion,
                Estatus = entity.Estatus
            };
            return response;
        }
    }
}
