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
    public class FormatosController : BaseController<FormatosController>
    {
        private readonly IMediator _mediator;

        public FormatosController(ILogger<FormatosController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Endpoint to Add a Formato
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Formato
        ///     ## Url
        ///         POST /Formatos/AñadirFormato
        /// </remarks>
        /// <paramref name="_formato"/> (FormatosRequest) indicated the info to be add
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
        public async Task<ActionResult> AgregarFormato(FormatosRequest formato)
        {
            try
            {
                var data = await _mediator.Send(new AgregarFormatoCommand(formato));
                var response = BuildOkResponse(data, HttpStatusCode.Created);
                return Created(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
        }

        /// <summary>
        ///     Endpoint to consult Formatos
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Formatos
        ///     ##Url
        ///         GET /Formatos/{_servicio}
        /// <paramref name="servicio"/> (string) that indicates the formato to be queried
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///     Failed:
        ///         - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>A list of FormatosResponse in the generic Response format</returns>
        [HttpGet("{servicio}")]
        [Authorize(Roles = "Prestador,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Response<FormatosResponse>>>> ConsultaFormatos(string servicio)
        {
            try
            {
                var data = await _mediator.Send(new ConsultarFormatoQuery(servicio));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.NotFound);
                return NotFound(response);
            }
        }

        /// <summary>
        ///     Endpoint to update Formato
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Patch Formato
        ///     ## Url
        ///         Patch /Formatos/UpdateFormato
        /// </remarks>
        /// <paramref name="_servicio"/> (FormatosRequest) that indicates the formato to be update
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///     Failed:
        ///         - Operation failure,  indicates the type of error and the error's reason.
        /// </response>
        /// <returns>Returns a generic Response with the result of the operation</returns>
        /// 
        [HttpPut("Update/{servicio}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateFormato(FormatosRequest servicio)
        {
            try
            {
                var data = await _mediator.Send(new UpdateFormatosCommand(servicio));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<string>(ex.GetErrorMessage(), HttpStatusCode.Conflict);
                return Conflict(response);
            }
        }
    }
}
