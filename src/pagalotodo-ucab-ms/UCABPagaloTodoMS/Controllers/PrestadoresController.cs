using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Net;
using UCABPagaloTodoMS.Application.BusinessValidation.Implementation;
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
    public class PrestadoresController : BaseController<PrestadoresController>
    {
        private readonly IMediator _mediator;

        public PrestadoresController(ILogger<PrestadoresController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Endpoint to consult Prestadores
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get prestadores
        ///     ##Url
        ///         GET /Prestadores/ConsultPrestadores
        /// <paramref name="username"/> (string) optional value, indicates the user to be queried
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        ///         - If _username is null, List of all PrestadoresResponse
        ///           If _username is not null, just user's info
        /// </response>
        /// <response code = "400" >
        ///     Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>A list of PrestadoresResponse in the generic Response format</returns>
        [HttpGet("username")]
        [Authorize(Roles = "Prestador,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<PrestadoresResponse>>>> ConsultaPrestador(string username)
        {
            try
            {
                //The consult generates a OkResult
                var data = await _mediator.Send(new ConsultarPrestadorQuery(username));
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
        ///     Endpoint to Add a Prestador
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Prestadores
        ///     ## Url
        ///         POST /Prestadores/AñadirPrestador
        /// </remarks>
        /// <paramref name="prestador"/> (PrestadorRequest) optional value, indicates the user to be add
        /// <response code="201">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregarPrestador(PrestadorRequest prestador)
        {
            try
            {
                //The add generates a OkResult
                var data = await _mediator.Send(new AgregarPrestadorCommand(prestador));
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
        ///     Endpoint to change the status
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Put Prestadores
        ///     ## Url
        ///         PUT /Prestadores/UpdateStatusPrestador
        /// </remarks>
        ///  <paramref name="consumidor"/> (StatusUserRequest) optional value, indicates the user to be update
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="409">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("UpdateStatus/{username}")]
        [Authorize(Roles = "Prestador,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateStatusPrestador(StatusUserRequest consumidor, string username)
        {
            try
            {
                var data = await _mediator.Send(new StatusUserCommand(consumidor, new PrestadorStatusUserValidation(), username));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.Conflict);
                return Conflict(response);
            }
        }


        /// <summary>
        ///     Endpoint change the password
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///     ### Post change the password
        ///     ## Url
        ///     POST /Prestadores/CambioClavePrestador
        /// </remarks>
        /// <paramref name="clavePrestador"/> (CambioClaveUserRequest) has prestador values to change the password
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code="409">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("CambioClave/{username}")]
        [Authorize(Roles = "Prestador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CambioClavePrestador(CambioClaveUserRequest clavePrestador, string username)
        {
            try
            {
                //The change generates a OkResult
                var data = await _mediator.Send(new CambioClaveUserCommand(clavePrestador, new PrestadorCambiarClaveValidation(), username));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                //The change throw a exception
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.Conflict);
                return Conflict(response);
            }
        }
        /// <summary>
        ///     Endpoint to update prestadores
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Patch Prestadores
        ///     ## Url
        ///         Patch /Prestadores/UpdatePrestador
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="409">
        ///     Failed:
        ///         - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>Returns a generic Response with the result of the operation</returns>
        /// 
        [HttpPut("UpdatePrestador/{username}")]
        [Authorize(Roles = "Administrador,Prestador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdatePrestador(PrestadorRequest prestador, string username)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new UpdatePrestadoresCommand(prestador, username));
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
        ///     Endpoint to consult Servicios
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Servicios
        ///     ##Url
        ///         GET /Servicios/ConsultServicios
        /// <paramref name="username"/> (string) optional value, indicates the user to be consult
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        ///         - If _username is null, throw exception
        ///           If _servicio is not null, just service's de prestador
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns> list of ServiciosResponse in the generic Response format</returns>
        [HttpGet("{username}/Servicios")]
        [Authorize(Roles = "Prestador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Response<ServiciosResponse>>>> ConsultaServiciosPorPrest(string username)
        {
            try
            {
                //The consult generates a OkResult
                var data = await _mediator.Send(new ConsultarServicioQuery(username, "prestador"));
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
    }
}
