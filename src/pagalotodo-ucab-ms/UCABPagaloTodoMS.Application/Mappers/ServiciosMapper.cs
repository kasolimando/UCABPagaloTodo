using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static UCABPagaloTodoMS.Core.Entities.ServicioEntity;

namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class ServiciosMapper
    {
        /// <summary>
        ///     Change a Servicios entity to a ServiciosResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: Servicios with the information to register
        /// </remarks>
        /// <returns>Returns the ServiciosResponse with the new data</returns>
        /// 
        public static ServiciosResponse MapEntityAResponse(ServicioEntity entity)
        {
            var response = new ServiciosResponse()
            {
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Estatus = Enum.GetName(typeof(Status),entity.Estatus),
                Categoria = entity.Categoria,
                TipoPago = entity.TipoPago,
                PrestadorEntityId = entity.PrestadorEntityId

            };
            return response;
        }

        /// <summary>
        ///     Change a ServicioRequest to a Servicios
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: ServicioRequest with the information to register
        /// </remarks>
        /// <returns>Returns the Servicios with the new data</returns>
        /// 
        public static ServicioEntity MapRequestEntity(ServicioRequest request)
        {
            var entity = new ServicioEntity()
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Estatus = Enum.Parse<Status>(request.Estatus.ToLower()),
                Categoria = request.Categoria,
                TipoPago = request.TipoPago,
                PrestadorEntityId = request.PrestadorEntityId
            };
            return entity;
        }


        /// <summary>
        ///     Switch the New values with the old values
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actual: The Servicios to be update
        ///         - entity: The Servicios with the new information
        /// </remarks>
        /// <returns>Returns the Servicios with the new data</returns>
        /// 
        public static ServicioEntity MapRequestUpdateEntity(ServicioEntity actual, ServicioEntity entity)
        {
            actual.Descripcion = entity.Descripcion;
            actual.Estatus = entity.Estatus;
            actual.Categoria = entity.Categoria;
            actual.TipoPago = entity.TipoPago;
            actual.PrestadorEntityId = entity.PrestadorEntityId;
            return actual;
        }

        /// <summary>
        ///     Switch the New values with the old values
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actual: The Servicios to be update
        ///         - entity: The Servicios with the new information
        /// </remarks>
        /// <returns>Returns the Servicios with the new data</returns>
        /// 
        public static ServicioEntity MapRequestUpdateEntitySinStatus(ServicioEntity actual, ServicioEntity entity)
        {
            actual.Descripcion = entity.Descripcion;
            actual.Categoria = entity.Categoria;
            actual.TipoPago = entity.TipoPago;
            actual.PrestadorEntityId = entity.PrestadorEntityId;
            return actual;
        }
    }
}
