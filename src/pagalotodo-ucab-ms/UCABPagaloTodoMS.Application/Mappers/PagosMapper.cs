using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Mappers
{
    public class PagosMapper
    {
        /// <summary>
        ///     Change a PagoRequest to a PagoEntity with a deuda
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: PagoRequest with the information to register
        /// </remarks>
        /// <returns>Returns the PagoEntity with the new data</returns>
        /// 
        public static PagoEntity MapRequestEntityWithADeuda(PagoRequest request, Guid _Id, bool _Aprobado, bool _Cierre, double Monto)
        {
            var entity = new PagoEntity()
            {
                ConsumidorEntityId = request.Consumidor,
                Monto = Monto,
                Aprobado = _Aprobado,
                Cierre = _Cierre,
                Fecha = DateTime.Now,
                ServicioEntityId = _Id
            };
            return entity;
        }

        /// <summary>
        ///     Change a Pagos entity to a PagosResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - entity: Pagos with the information to register
        /// </remarks>
        /// <returns>Returns the PagoResponse with the new data</returns>
        /// 
        public static PagoResponse MapEntityAResponse(PagoEntity entity, string _nombre)
        {
            var response = new PagoResponse()
            {
                Consumidor = entity.ConsumidorEntityId,
                Monto = entity.Monto,
                Fecha = entity.Fecha.ToString(),
                Aprobado = entity.Aprobado,
                Cierre = entity.Cierre,
                Servicio = _nombre

            };
            return response;
        }
    }
}