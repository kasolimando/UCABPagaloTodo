using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AdministradoresController : BaseController<AdministradoresController>
    {
        private readonly IMediator _mediator;

        public AdministradoresController(ILogger<AdministradoresController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;   //The instance to use the MediatR
        }

        /// <summary>
        ///     Endpoint to consult the Administrador
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Administradores
        ///     ## Url
        ///         GET /Administradores/{_username}
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code = "404" >
        ///     Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>A list of AdminsReponse in the generic Response format</returns>
        [HttpGet("{username}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<AdminsResponse>>> ConsultaAdmin(string username)
        {
            try
            {
                //The consult generates a OkResult
                var data = await _mediator.Send(new ConsultarAdminsQuery(username));
                var response = BuildOkResponse(data,HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                //The consult throw a exception
                var response = BuildBadResponse<AdminsResponse>(ex.GetErrorMessage(), HttpStatusCode.NotFound);
                return NotFound(response);
            }
        }

        /// <summary> 
        ///     Endpoint to update an Administrador
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         PUT update Administrador
        ///     ## Url
        ///         PUT /Administradores/
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code="409">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPut("Update/{username}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateAdmin(AdminsRequest admin, string username)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new UpdateAdminsCommand(admin, username));
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
        ///     Endpoint to update the admin status
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         PATCH update the admin status
        ///     ## Url
        ///         PATCH /Administradores/UpdateStatusAdmin
        /// </remarks>
        /// <response code="200">
        ///     Accepted:
        ///     - Operation successful.
        /// </response>
        /// <response code="409">
        ///    Failed:
        ///     - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>The generic Response format with the operation result</returns>
        [HttpPatch("UpdateStatus/{username}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateStatusAdmin(StatusUserRequest admin, string username)
        {
            try
            {
                //The update generates a OkResult
                var data = await _mediator.Send(new StatusUserCommand(admin,new AdminStatusUserValidation(), username));
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
        ///     Endpoint change the administrator password
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///     ### PATCH change the administrator password
        ///     ## Url
        ///     PATCH /Administradores/CambioClaveAdministrador
        /// </remarks>
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
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CambioClaveAdministrador(CambioClaveUserRequest claveAdmin, string username)
        {
            try
            {
                //The change generates a OkResult
                var data = await _mediator.Send(new CambioClaveUserCommand(claveAdmin, new AdminCambiarClaveValidation(), username));
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
