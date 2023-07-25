using BbcTravelMS.Entities;
using BbcTravelMS.Models.Starter;
using BbcTravelMS.Models.Validation;
using BbcTravelMS.Services.Interface;
using BbcTravelMS.Settings;
using Microsoft.AspNetCore.Mvc;
using NASSA.Utils.BaseController;
using Newtonsoft.Json;

namespace BbcTravelMS.Controllers;

[ApiController]
[Route("[controller]")]
public class StarterController : BaseController<StarterController>
{
    private const string StarterErrorMessage = "An unexpected error occurred while trying to get the starter.";
    private readonly IStarterService _starterService;

    public StarterController(ILogger<StarterController> logger, IStarterService starterService) : base(logger)
    {
        _starterService = starterService;
    }

    #region "GetStarterList"

    /// <summary>
    ///     Endpoint for getting pricing by productId.
    /// </summary>
    /// <remarks>
    ///     ## Description
    ///     ### Get Starter List
    ///     ## Url
    ///     GET /starter/GetStarterList
    ///     ## Required Fields
    ///     #### Response
    ///     {
    ///     "operationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "operationName": "string",
    ///     "data": [
    ///     {
    ///     "createdAt": "2021-02-08T19:19:30.520Z",
    ///     "createdBy": 0,
    ///     "updatedAt": "2021-02-08T19:19:30.520Z",
    ///     "updatedBy": 0,
    ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "name": "string"
    ///     }
    ///     ],
    ///     "pagination": {
    ///     "next": 0,
    ///     "previous": 0,
    ///     "total": 0
    ///     }
    ///     }
    /// </remarks>
    /// <response code="200">
    ///     Accepted:
    ///     - Operation successful.
    /// </response>
    /// <returns>Get a starter list.</returns>
    [HttpGet("GetStarterList")]
    [ProducesResponseType(typeof(Response200<List<StarterEntity>>), 200)]
    [ProducesResponseType(typeof(Response400), 400)]
    [ProducesResponseType(typeof(Response404), 404)]
    public async Task<ActionResult<List<StarterEntity>>> GetStarterList()
    {
        _logger.LogInformation(LoggingEvents.GetStarterList, "Getting List: {@StarterEntity}", new StarterEntity());
        try
        {
            var starterList = await _starterService.GetStarterList();

            _logger.LogInformation(LoggingEvents.GetStarterList, "Information: inside scope controller");
            _logger.LogWarning(LoggingEvents.GetStarterList, "Warning: inside scope controller");
            _logger.LogError(LoggingEvents.GetStarterList, new Exception("From starter"), "Error: inside scope controller");

            return Response200(NewResponseOperation(), starterList);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetStarterListError, ex, StarterErrorMessage);
            return Response400(StarterErrorMessage, ex.Message);
        }
    }

    #endregion

    #region "CreateStarter"

    /// <summary>
    ///     Endpoint for creating a new starter entity
    /// </summary>
    /// <remarks>
    ///     ## Description
    ///     ### POST Starter
    ///     ## Url
    ///     POST /starter/CreateStarter
    ///     ## Required Fields
    ///     {
    ///     "createdAt": "2021-02-08T19:19:30.520Z",
    ///     "createdBy": 0,
    ///     "updatedAt": "2021-02-08T19:19:30.520Z",
    ///     "updatedBy": 0,
    ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "name": "string"
    ///     }
    ///     #### Response
    ///     {
    ///     "operationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "operationName": "string",
    ///     "data": [
    ///     {
    ///     "createdAt": "2021-02-08T19:19:30.520Z",
    ///     "createdBy": 0,
    ///     "updatedAt": "2021-02-08T19:19:30.520Z",
    ///     "updatedBy": 0,
    ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "name": "string"
    ///     }
    ///     ],
    ///     "pagination": {
    ///     "next": 0,
    ///     "previous": 0,
    ///     "total": 0
    ///     }
    ///     }
    /// </remarks>
    /// <response code="201">
    ///     Accepted:
    ///     - Operation successful.
    /// </response>
    /// <returns>Get a object with coverages and limits.</returns>
    [HttpPost("CreateStarter")]
    [ProducesResponseType(typeof(Response201<StarterEntity>), 201)]
    [ProducesResponseType(typeof(Response422), 422)]
    [ProducesResponseType(typeof(Response400), 400)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StarterEntity>> CreateStarter([FromBody] StarterRequest starterRequest)
    {
        _logger.LogInformation(LoggingEvents.CreateStarterItem, "Create Starter. StarterRequest : {StarterRequest}",
            JsonConvert.SerializeObject(starterRequest));
        try
        {
            StarterRequestValidator validator = new();
            var result = validator.Validate(starterRequest);
            if (!result.IsValid)
            {
                foreach (var errorItem in result.Errors)
                {
                    ModelState.AddModelError(errorItem.PropertyName, errorItem.ErrorMessage);
                }

                return UnprocessableEntity(ModelState);
            }

            var starterEntity = starterRequest;
            StarterEntity starter = new() { Id = starterEntity.Id, Name = starterEntity.Name };
            var createdResult = await _starterService.CreateStarterAsync(starter);
            return Response201(NewResponseOperation(), createdResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.CreateStarterError, ex, StarterErrorMessage);
            return Response400(StarterErrorMessage, ex.Message);
        }
    }

    #endregion

    #region "PublishTopic"

    /// <summary>
    ///     Endpoint for creating a new starter entity
    /// </summary>
    /// <remarks>
    ///     ## Description
    ///     ### POST Starter
    ///     ## Url
    ///     POST /starter/PublishTopic
    ///     ## Required Fields
    ///     {
    ///     "createdAt": "2021-02-08T19:19:30.520Z",
    ///     "createdBy": 0,
    ///     "updatedAt": "2021-02-08T19:19:30.520Z",
    ///     "updatedBy": 0,
    ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "name": "string"
    ///     }
    ///     #### Response
    ///     {
    ///     "operationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "operationName": "string",
    ///     "data": [
    ///     {
    ///     "createdAt": "2021-02-08T19:19:30.520Z",
    ///     "createdBy": 0,
    ///     "updatedAt": "2021-02-08T19:19:30.520Z",
    ///     "updatedBy": 0,
    ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "name": "string"
    ///     }
    ///     ],
    ///     "pagination": {
    ///     "next": 0,
    ///     "previous": 0,
    ///     "total": 0
    ///     }
    ///     }
    /// </remarks>
    /// <response code="204">
    ///     Accepted:
    ///     - Operation successful.
    /// </response>
    /// <returns>Get a object with coverages and limits.</returns>
    [HttpPost("PublishTopic")]
    [ProducesResponseType(typeof(Response201<StarterEntity>), 204)]
    [ProducesResponseType(typeof(Response422), 422)]
    [ProducesResponseType(typeof(Response400), 400)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> PublishTopic([FromBody] StarterRequest starterRequest)
    {
        _logger.LogInformation(LoggingEvents.CreateStarterItem, "Create Starter. StarterRequest : {StarterRequest}",
            JsonConvert.SerializeObject(starterRequest));
        try
        {
            StarterRequestValidator validator = new();
            var result = await validator.ValidateAsync(starterRequest);
            if (result.IsValid)
            {
                return Response204();
            }

            foreach (var errorItem in result.Errors)
            {
                ModelState.AddModelError(errorItem.PropertyName, errorItem.ErrorMessage);
            }

            return UnprocessableEntity(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.CreateStarterError, ex, StarterErrorMessage);
            return Response400(StarterErrorMessage, ex.Message);
        }
    }

    #endregion

    #region "CalculateAgeRate"

    /// <summary>
    ///     Endpoint for getting the starter age.
    /// </summary>
    /// <remarks>
    ///     ## Description
    ///     ### POST Starter Age
    ///     ## Url
    ///     POST /starter/GetStarterDetail/Age
    ///     ## Required Fields
    ///     {
    ///     "birthDay": "2021-02-08T19:19:30.520Z",
    ///     }
    ///     ## Required Fields
    ///     #### Response
    ///     {
    ///     "operationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "operationName": "string",
    ///     "data": [
    ///     {
    ///     "age": 0
    ///     }
    ///     ],
    ///     "pagination": {
    ///     "next": 0,
    ///     "previous": 0,
    ///     "total": 0
    ///     }
    /// </remarks>
    /// <response code="200">
    ///     Accepted:
    ///     - Operation successful.
    /// </response>
    /// <returns>Get a starter age.</returns>
    [HttpPost("GetStarterDetail/Age")]
    [ProducesResponseType(typeof(Response200<int>), 200)]
    [ProducesResponseType(typeof(Response400), 400)]
    [ProducesResponseType(typeof(Response404), 404)]
    public ActionResult<int> CalculateAgeRate([FromBody] AgeRateRequest ageRateRequest)
    {
        _logger.LogInformation(LoggingEvents.CalculateAgeRate, "CalculateAgeRate Age: {AgeRateRequest}",
            JsonConvert.SerializeObject(ageRateRequest));
        try
        {
            AgeRateRequestValidator validator = new();
            var result = validator.Validate(ageRateRequest);
            if (!result.IsValid)
            {
                foreach (var errorItem in result.Errors)
                {
                    ModelState.AddModelError(errorItem.PropertyName, errorItem.ErrorMessage);
                }

                return UnprocessableEntity(ModelState);
            }

            var starterAge = _starterService.CalculateAgeRate(ageRateRequest.BirthDay);
            return Response200(NewResponseOperation(), starterAge);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.CalculateAgeRateError, ex, StarterErrorMessage);
            return Response400(StarterErrorMessage, ex.Message);
        }
    }

    #endregion
}
