using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class PrestadoresMapper
    {

        /// <summary>
        ///     Change a PrestadorEntity entity to a PrestadoresResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: PrestadorEntity with the information to register
        /// </remarks>
        /// <returns>Returns the PrestadoresResponse with the new data</returns>
        /// 
        public static PrestadoresResponse MapEntityAResponse(PrestadorEntity entity)
        {
            var response = new PrestadoresResponse()
            {
                Username = entity.Username,
                Correo = entity.Correo,
                Nombre = entity.Nombre+" "+entity.Apellido,
                DocIdentidad = entity.TipoVj +"-"+ entity.DocIdentidad,
                Direccion = entity.Direccion,
                Estatus = entity.Estatus

            };
            return response;
        }


        /// <summary>
        ///     Change a PrestadorRequest to a PrestadorEntity
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: PrestadorRequest with the information to register
        /// </remarks>
        /// <returns>Returns the PrestadorEntity with the new data</returns>
        /// 
        public static PrestadorEntity MapRequestEntity(PrestadorRequest request)
        {
            var entity = new PrestadorEntity()
            {
                Username = request.Username,
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
        ///         - actual: The PrestadorEntity to be update
        ///         - entity: The PrestadorEntity with the new information
        /// </remarks>
        /// <returns>Returns the PrestadorEntity with the new data</returns>
        /// 
        public static PrestadorEntity MapRequestUpdateEntity(PrestadorEntity actual, PrestadorEntity entity)
        {
            actual.DocIdentidad = entity.DocIdentidad;
            actual.TipoVj = entity.TipoVj;
            actual.Correo = entity.Correo;
            actual.Direccion = entity.Direccion;
            actual.Nombre = entity.Nombre;
            actual.Apellido = entity.Apellido;
            return actual;
        }
    }
}
