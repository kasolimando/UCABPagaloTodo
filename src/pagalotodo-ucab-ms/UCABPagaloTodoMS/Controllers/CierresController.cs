using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;


namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CierresController : BaseController<CierresController>
    {
        private readonly IMediator _mediator;

        public CierresController(ILogger<CierresController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Endpoint to Make a Cierre
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Cierre
        ///     ## Url
        ///         POST /Cierres/CierreContable
        /// </remarks>
        ///  <paramref name="_servicio"/> (string) indicates the service that the accounting closing will be done
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
        public async Task<ActionResult> CierreContable(string servicio)
        {
            try
            {
                //The cierre generates a OkResult
                var data = await _mediator.Send(new CierreCommand(servicio));
                var response = BuildOkResponse(data, HttpStatusCode.Created);
                return Created(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<AdminsResponse>(ex.GetErrorMessage(), HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
        }
    }
}
