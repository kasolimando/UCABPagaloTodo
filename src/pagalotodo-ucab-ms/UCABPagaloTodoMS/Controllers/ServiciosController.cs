using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Data;
using System.Net;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;


namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiciosController : BaseController<ServiciosController>
    {
        private readonly IMediator _mediator;

        public ServiciosController(ILogger<ServiciosController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Endpoint to Add a Servicio
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Servicio
        ///     ## Url
        ///         POST /Servicios/AñadirServicio
        /// </remarks>
        /// <paramref name="servicio"/> (ServicioRequest) has servicio values to be add
        /// <response code="201">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///     Failed:
        ///         - Operation failure, indicates the type of error and the error's reason.
        /// </response>
        /// <returns>Returns a generic Response with the result of the operation</returns>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregarServicio(ServicioRequest servicio)
        {
            try
            {
                //The add generates a OkResult
                var data = await _mediator.Send(new AgregarServicioCommand(servicio));
                var response = BuildOkResponse(data, HttpStatusCode.Created);
                return Created(response);
            }
            catch (CustomException ex)
            {
                //The add throw a exception
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
        }

        /// <summary>
        ///     Endpoint to consult Servicios
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Servicios
        ///     ##Url
        ///         GET /Servicios/ConsultServicios
        /// <paramref name="_servicio"/> (string) optional value, that indicates the servicio to be queried
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        ///         - If _servicio is null, List of all Servicios
        ///           If _servicio is not null, just service's info
        /// </response>

        /// <response code="404">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>list of ServiciosResponse in the generic Response format</returns>
        /// 
        [HttpGet("servicio")]
        [Authorize(Roles = "Consumidor,Administrador,Prestador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<ServiciosResponse>>>> ConsultaServicio(string servicio)
        {
            try
            {
                //The consult generates a OkResult
                var data = await _mediator.Send(new ConsultarServicioQuery(servicio, "servicio"));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                //The consult throw a exception
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.NotFound);
                return NotFound(response);
            }
        }

        /// <summary>
        ///     Endpoint to update Servicios
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Put Servicios
        ///     ## Url
        ///         PUT /Servicios/UpdateServicio
        /// </remarks>
        /// <paramref name="_servicio"/> (ServicioRequest) that indicates the servicio to be update
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPut("Update/{servicio}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateServicio(ServicioRequest servicio)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new UpdateServiciosCommand(servicio));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                //The update throw a exception
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.Conflict);
                return Conflict(response);
            }
        }

        /// <summary>
        ///     Endpoint para actualizar el estatus prestadores
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Put Prestadores
        ///     ## Url
        ///         PUT /Prestadores/UpdateStatusPrestador
        /// </remarks>
        /// <paramref name="_servicio"/> (StatusServicioRequest) indicates the servicio to be update
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("UpdateStatus/{nombreServicio}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateStatusServicio(StatusServicioRequest servicio, string nombreServicio)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new StatusServiciosCommand(servicio, nombreServicio));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                //The update throw a exception
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.Conflict);
                return Conflict(response);
            }
        }
    }
}
