using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using System.Net;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;

namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController<LoginController>
    {
        private readonly IMediator _mediator;

        public LoginController(ILogger<LoginController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;   // se utiliza para poder comunicarse con el mediador
        }

        /// <summary>
        ///     Endpoint to consult the usernames
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get usuarios
        ///     ## Url
        ///         GET Login/{_username}/{_clave}
        /// </remarks>
        /// <paramref name="username"/> (string) Indicates the user to be queried
        /// <paramref name="clave"/> (string) Indicates the password to be queried
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code = "400" >
        ///     Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>list of AdminsReponse in the generic Response format</returns>
        [HttpGet("{username}/{clave}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<LoginResponse>>> LoginUsuarios(string username, string clave)
        {
            try
            {
                //The consult generates a OkResult
                //the response fields are filled
                var data = await _mediator.Send(new LoginUsuariosQuery(username, clave));
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
