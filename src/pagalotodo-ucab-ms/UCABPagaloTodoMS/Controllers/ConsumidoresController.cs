using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Data;
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
    public class ConsumidoresController : BaseController<ConsumidoresController>
    {
        private readonly IMediator _mediator;

        public ConsumidoresController(ILogger<ConsumidoresController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Endpoint to consult Consumidores
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get consumidores
        ///     ##Url
        ///         GET /Consumidores/ConsultConsumidor
        /// <paramref name="username"/> string? is an optional values, that indicates the user to be queried
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        ///         - If _username is null, List of all ConsumidoresResponse
        ///           If _username is not null, just user's info
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>list of ConsumidoresResponse in the generic Response format</returns>
        /// 
        [HttpGet("username")]
        [Authorize(Roles = "Consumidor,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<ConsumidoresResponse>>>> ConsultaConsumidor(string username)
        {
            try
            {
                //The consult generates a OkResult
                var data = await _mediator.Send(new ConsultarConsumidorQuery(username));
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
        ///     Endpoint to Add a Consumidor
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post of consumidores
        ///     ## Url
        ///         POST /Consumidores
        /// </remarks>
        /// <paramref name="consumidor"/>  (ConsumidorRequest) that indicates the user to be add
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregarConsumidor(ConsumidorRequest consumidor)
        {
            try
            {
                //The add generates a OkResult
                var data = await _mediator.Send(new AgregarConsumidorCommand(consumidor));
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
        ///     Endpoint to update consumidores
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         PUT of consumidores
        ///     ## Url
        ///         PUT /Consumidores/Update
        /// </remarks>
        /// <paramref name="consumidor"/>  (ConsumidorRequest) that indicates the user to be updated
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        /// 
        [HttpPut("Update/{username}")]
        [Authorize(Roles = "Consumidor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateConsumidor(ConsumidorRequest consumidor, string username)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new UpdateConsumCommand(consumidor, username));
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
        ///     Endpoint to update the consumdor status
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Put consumidores
        ///     ## Url
        ///         PUT /Consumidores/UpdateStatus
        /// </remarks>
        /// <paramref name="consumidor"/>  (StatusUserRequest) that indicates the user to be update
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("UpdateStatus/{username}")]
        [Authorize(Roles = "Consumidor,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateStatusConsumidor(StatusUserRequest consumidor, string username)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new StatusUserCommand(consumidor,new ConsumidorStatusUserValidation(), username));
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
        ///     Endpoint change the consumidor password
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post change the consumidor password
        ///     ## Url
        ///         POST /Consumidores/CambioClaveConsumidor
        /// </remarks>
        /// <paramref name="claveConsumidor"/> (CambioClaveUserRequest) indicated the info to change the password
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("CambioClave/{username}")]
        [Authorize(Roles = "Consumidor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CambioClaveConsumidor(CambioClaveUserRequest claveConsumidor, string username)
        {
            try
            {
                //The change generates a OkResult
                var data = await _mediator.Send(new CambioClaveUserCommand(claveConsumidor, new ConsumidorCambiarClaveValidation(), username));
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
        ///     Endpoint to consult Deudas
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Deudas
        ///     ##Url
        ///         GET /Deudas/{_servicio}
        /// <parameters>
        ///     <paramref name="servicio"/> (string) indicates the service's format to be queried
        ///     <paramref name="username"/> (string?) is an optional values, indicates the username to be queried
        /// </parameters>
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///     Failed:
        ///         - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>A list of DeudaResponse in the generic Response format</returns>
        [HttpGet("{username}/Deudas")]
        [Authorize(Roles = "Consumidor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<DeudaResponse>>>> ConsultaDeudas(string username)
        {
            try
            {
                var data = await _mediator.Send(new ConsultarDeudasQuery(string.Empty, username));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.NotFound);
                return NotFound(response);
            }
        }
    }
}
