using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;



namespace UCABPagaloTodoMS.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class FormatoMapper
    {

        /// <summary>
        ///     Change a FormatoConEntityto a FormatosResponse
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - nombre: string with the name of the Servicio
        ///         - campos : List<string> with the name of the campos
        ///         - longitud : List<int> with the lenght of the campos
        /// </remarks>
        /// <returns>Returns the FormatosResponse with the new data</returns>
        /// 
        public static FormatosResponse MapEntityAResponse(string campos, int longitud, bool requerido, string servicio)
        {
            var response = new FormatosResponse()
            {
                Campos = campos,
                Longitud = longitud,
                Requerido = requerido,
                Servicio = servicio

            };
            return response;
        }

        /// <summary>
        ///     Change a FormatosRequest to a FormatoConEntity
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - request: FormatosRequest with the information to register
        ///         - ID: Guid of the Servicio
        /// </remarks>
        /// <returns>Returns the FormatoConEntity with the new data</returns>
        /// 
        public static FormatoServicioEntity MapRequestEntity(int longitud, Guid idCampo, Guid idServicio, bool requerido)
        {
            var entity = new FormatoServicioEntity()
            {
                Requerido = requerido,
                ServicioEntityId = idServicio,
                FormatoConEntityId = idCampo,
                Logitud = longitud

            };
            return entity;
        }

        /// <summary>
        ///     Switch the New values with the old values
        /// </summary>
        /// <remarks>
        ///     ## Parameters
        ///         - actual: The FormatoConEntity to be update
        ///         - entity: The FormatoConEntity with the new information
        /// </remarks>
        /// <returns>Returns the FormatoConEntity with the new data</returns>
        /// 
        public static FormatoServicioEntity MapRequestUpdateEntity(FormatoServicioEntity actual, FormatoServicioEntity entity)
        {
            actual.Logitud = entity.Logitud;
            actual.Requerido = entity.Requerido;
            return actual;
        }
    }
}
