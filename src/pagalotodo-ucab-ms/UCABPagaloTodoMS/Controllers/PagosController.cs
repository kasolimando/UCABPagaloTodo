using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public class PagosController : BaseController<PagosController>
        {
            private readonly IMediator _mediator;

            public PagosController(ILogger<PagosController> logger, IMediator mediator) : base(logger)
            {
                _mediator = mediator;
            }


        /// <summary>
        ///     Endpoint to Add a pago
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post pago
        ///     ## Url
        ///         POST /Pagos/AñadirPago
        /// </remarks>
        /// <response code="201">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code = "400" >
        ///     Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPost]
        [Authorize(Roles = "Consumidor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregarPago(PagoRequest pago)
        {
            try
            {
                //The add generates a OkResult
                var data = await _mediator.Send(new AgregarPagoCommand(pago));
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
        ///     Endpoint to consult Pagos
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Pagos
        ///     ##Url
        ///         GET /Pagos/ConsultarPagos {_fecha1}/{_fecha2}
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///         - Operation successful.
        ///         - If _servicio is null, show all pagos.         
        /// </response>
        /// <response code="404">
        ///     Failed:
        ///         - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>A list of PagoResponse in the generic Response format</returns>

        [HttpPost("ConsultarPagos")]
        [Authorize(Roles = "Consumidor,Administrador,Prestador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<PagoResponse>>>> ConsultarPagos(ConsultarPagosRequest request)
        {
            try
            {
                //The consult generates a OkResult
                //the response fields are filled
                var data = await _mediator.Send(new ConsultarPagoQuery(request.Servicio, request.Consumidor, request.fechaInicio, request.fechaFin));
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
