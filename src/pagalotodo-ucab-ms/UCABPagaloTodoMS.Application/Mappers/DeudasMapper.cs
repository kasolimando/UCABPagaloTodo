using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.DeudaEntity;

namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class DeudasMapper
    {
        /// <summary>
        ///     Change a Deuda entity to a DeudaResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: Deuda with the information to register
        /// </remarks>
        /// <returns>Returns the DeudaResponse with the new data</returns>
        /// 
        public static DeudaResponse MapEntityAResponse(DeudaEntity entity, string _nombre)
        {
            var response = new DeudaResponse()
            {
                Consumidor = entity.Username,
                Monto = entity.Monto,
                Estatus = Enum.GetName(typeof(Status),entity.Estatus),
                Servicio = _nombre
            };
            return response;
        }

        /// <summary>
        ///     Change a Deuda entity to a DeudaResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: Deuda with the information to register
        /// </remarks>
        /// <returns>Returns the DeudaRequest with the new data</returns>
        /// 

        public static DeudaEntity MapRequestAEntity(string _username, Guid _servicio, Double _monto)
        {
            var entity = new DeudaEntity()
            {
                Username = _username,
                Monto = _monto,
                servicioId = _servicio,
                Estatus = Status.Activo,
            };
            return entity;
        }


    }
}
