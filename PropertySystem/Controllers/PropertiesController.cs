using BusinessLogic.Contracts;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService PropertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        PropertyService = propertyService;
    }

    /// <summary>
    /// Retrieves a property by its unique identifier.
    /// </summary>
    /// <param name="propertyId">The unique ID of the property.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The details of the property.</returns>
    /// <response code="200">Returns the property data.</response>
    /// <response code="404">If the property is not found.</response>
    [HttpGet("{propertyId}")]
    [ProducesResponseType(typeof(PropertyDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetPropertyById(Guid propertyId, CancellationToken cancellationToken)
    {
        var res = await PropertyService
            .GetPropertyByIdAsync(propertyId, cancellationToken);
        return res.ToActionResult();
    }

    /// <summary>
    /// Lists properties based on a filter.
    /// </summary>
    /// <param name="filterDto">Filter criteria for the properties.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A list of filtered properties.</returns>
    /// <response code="200">Returns the list of properties.</response>
    /// <response code="404">If the property is not found.</response>
    [HttpGet]
    [Route("list-properties")]
    [ProducesResponseType(typeof(IEnumerable<PropertyDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ListProperties([FromQuery] PropertyFilterDto filterDto, CancellationToken cancellationToken)
    {
        var res = await PropertyService
            .ListPropertiesAsync(filterDto, cancellationToken);
        return res.ToActionResult();
    }

    /// <summary>
    /// Creates a new property.
    /// </summary>
    /// <param name="propertyDto">The property details to be created.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Indicates whether the property was successfully created.</returns>
    /// <response code="201">If the property was successfully created.</response>
    /// <response code="400">If the request contains invalid data.</response>
    /// <response code="409">If there is a conflict, such as a duplicate property.</response>
    [HttpPost]
    [Route("create-property")]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.Conflict)] 
    public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyDto propertyDto, CancellationToken cancellationToken)
    {
        var res = await PropertyService
            .CreatePropertyAsync(propertyDto, cancellationToken);
        return res.ToActionResult();
    }

    /// <summary>
    /// Updates an existing property.
    /// </summary>
    /// <param name="propertyId">The unique ID of the property to update.</param>
    /// <param name="propertyDto">The new details of the property.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Indicates whether the property was successfully updated.</returns>
    /// <response code="200">If the property was successfully updated.</response>
    /// <response code="400">If the request contains invalid data.</response>
    /// <response code="404">If the property is not found.</response>
    /// <response code="409">If there is a conflict, such as a duplicate property.</response>
    [HttpPut]
    [Route("{propertyId}/update-property")]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdatePropertyAsync(Guid propertyId, [FromBody] UpdatePropertyDto propertyDto, CancellationToken cancellationToken)
    {
        var res = await PropertyService
            .UpdatePropertyAsync(propertyId, propertyDto, cancellationToken);
        return res.ToActionResult();
    }

    /// <summary>
    /// Updates the price of a property.
    /// </summary>
    /// <param name="propertyId">The unique ID of the property to update.</param>
    /// <param name="changePriceDto">The new price details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Indicates whether the price was successfully updated.</returns>
    /// <response code="200">If the price was successfully updated.</response>
    /// <response code="400">If the request contains invalid data.</response>
    /// <response code="404">If the property is not found.</response>
    /// <response code="409">If there is a conflict, such as a duplicate property.</response>
    [HttpPatch]
    [Route("{propertyId}/change-price")]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ChangePrice(Guid propertyId, [FromBody] ChangePriceDto changePriceDto, CancellationToken cancellationToken)
    {
        var res = await PropertyService
            .ChangePriceAsync(propertyId, changePriceDto, cancellationToken);
        return res.ToActionResult();
    }
}
