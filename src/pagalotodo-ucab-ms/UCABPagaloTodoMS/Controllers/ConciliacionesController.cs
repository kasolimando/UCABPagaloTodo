using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;


namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConciliacionesController : BaseController<ConciliacionesController>
    {
        private readonly IMediator _mediator;

        public ConciliacionesController(ILogger<ConciliacionesController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Endpoint to Make a Conciliacion
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Conciliacion
        ///     ## Url
        ///         POST /Conciliaciones
        /// </remarks>
        /// <paramref name="servicio"/> (CargarConciliacionRequest) indicates the name of the conciliation file
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
        [Authorize(Roles = "Prestador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Conciliacion(IFormFile Archivo)
        {
            try
            {
                //The conciliacion generates a OkResult
                var data = await _mediator.Send(new CargarConciliacionCommand(Archivo));
                var response = BuildOkResponse(data, HttpStatusCode.Created);
                return Created(response);
            }
            catch (CustomException ex)
            {
                //The conciliacion throw a exception
                var response = BuildBadResponse<AdminsResponse>(ex.GetErrorMessage(), HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
        }
    }
}
