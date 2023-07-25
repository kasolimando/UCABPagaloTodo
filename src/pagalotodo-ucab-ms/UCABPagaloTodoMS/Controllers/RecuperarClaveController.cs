using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Application.Commands;
using Microsoft.Azure.Amqp.Framing;

namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecuperarClaveController : BaseController<RecuperarClaveController>
    {
        private readonly IMediator _mediator;

        public RecuperarClaveController(ILogger<RecuperarClaveController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;   // se utiliza para poder comunicarse con el mediador
        }

        /// <summary>
        ///     Endpoint recover password
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Put recover password
        ///     ## Url
        ///         PUT /ReccuperarClave/{_username}
        /// </remarks>
        /// <paramref name="username"/> (string) has username values to recover password
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code="400">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> RecuperarClave(string username)
        {
            try
            {
                //The change generates a OkResult
                var data = await _mediator.Send(new RecuperarClaveCommand(username));
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
    
    }
}
