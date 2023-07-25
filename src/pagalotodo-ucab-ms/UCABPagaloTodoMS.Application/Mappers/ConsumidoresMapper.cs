using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class ConsumidoresMapper
    {

        /// <summary>
        ///     Change a Consumidor entity to a ConsumidoresReponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: ConsumidorEntity with the information to register
        /// </remarks>
        /// <returns>Returns the ConsumidoresResponse with the new data</returns>
        /// 

        public static ConsumidoresResponse MapEntityAResponse(ConsumidorEntity entity)
        {
            var response = new ConsumidoresResponse()
            {
                Username = entity.Username,
                Correo = entity.Correo,
                Nombre = entity.Nombre+" "+entity.Apellido,
                DocIdentidad = entity.TipoVj+"-"+entity.DocIdentidad,
                Direccion = entity.Direccion,
                Estatus = entity.Estatus
            };
            return response;
        }

        /// <summary>
        ///     Change a ConsumidorRequest to a ConsumidorEntity
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: ConsumidorRequest with the information to register
        /// </remarks>
        /// <returns>Returns the ConsumidorEntity with the new data</returns>
        /// 

        public static ConsumidorEntity MapRequestEntity(ConsumidorRequest request)
        {
            var entity = new ConsumidorEntity()
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
        ///         - actual: The ConsumidorEntity to be update
        ///         - entity: The ConsumidorEntity with the new information
        /// </remarks>
        /// <returns>Returns the ConsumidorEntity with the new data</returns>
        /// 
        public static ConsumidorEntity MapRequestUpdateEntity(ConsumidorEntity actual, ConsumidorEntity entity)
        {
            actual.DocIdentidad = entity.DocIdentidad;
            actual.TipoVj = entity.TipoVj;
            actual.Correo = entity.Correo;
            actual.Direccion = entity.Direccion;
            actual.Nombre = entity.Nombre;
            actual.Apellido = entity.Apellido;
            actual.Estatus = entity.Estatus;
            return actual;
        }


        public static ConsumidorEntity MapRequestEntityConsum(ConsumidorRequest request)
        {
            var entity = new ConsumidorEntity()
            {
                Username = request.Username,
                Correo = request.Correo,
                DocIdentidad = request.DocIdentidad,
                TipoVj = request.TipoVj,
                Direccion = request.Direccion ?? string.Empty,
                Nombre = request.Nombre
            };
            return entity;
        }
    }
}
