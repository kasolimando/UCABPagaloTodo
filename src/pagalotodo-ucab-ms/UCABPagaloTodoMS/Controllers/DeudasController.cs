using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;


namespace UCABPagaloTodoMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeudasController : BaseController<DeudasController>
    {
        private readonly IMediator _mediator;

        public DeudasController(ILogger<DeudasController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Endpoint to Add a Deuda
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Post Deuda
        ///     ## Url
        ///         POST /Deudas/AñadirDeuda
        /// </remarks>
        /// <paramref name="deuda"/> (IFormFile) indicated the info to be add
        /// <response code="201">
        ///     Accepted:
        ///         - Operation successful.
        /// </response>
        /// <response code="400">
        ///     Failed:
        ///         - Operation Failed due to Exception,indicates the type of error and the error's reason.
        /// </response>
        /// <returns>Returns a generic Response with the result of the operation</returns>
        [HttpPost]
        [Authorize(Roles = "Prestador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregarDeuda(IFormFile Archivo)
        {
            try
            {
                //The add generates a OkResult
                var data = await _mediator.Send(new AgregarDeudaCommand(Archivo));
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
        ///     Endpoint to consult Deudas
        /// </summary>
        /// <remarks>
        ///     ## Description
        ///         Get Deudas
        ///     ##Url
        ///         GET /Deudas/{_servicio}
        /// <parameters>
        ///     <paramref name="servicio"/> (string) indicates the service's format to be queried
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
        [HttpGet("{servicio}")]
        [Authorize(Roles = "Prestador,Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<List<DeudaResponse>>>> ConsultaDeudas(string servicio)
        {
            try
            {
                var data = await _mediator.Send(new ConsultarDeudasQuery(servicio, string.Empty));
                var response = BuildOkResponse(data, HttpStatusCode.OK);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                var response = BuildBadResponse<AdminsResponse>(ex.GetErrorMessage(), HttpStatusCode.NotFound);
                return NotFound(response);
            }
        }
    }
}
