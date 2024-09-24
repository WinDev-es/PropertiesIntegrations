using BusinessLogic.Contracts;
using DataTransferObjects.Dto.Request;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly IPropertyImageService propertyImageService;

    public PropertyImagesController(IPropertyImageService propertyImageService)
    {
        this.propertyImageService = propertyImageService;
    }

    /// <summary>
    /// Gets all images associated with a specific property by its ID.
    /// </summary>
    /// <param name="propertyId">The ID of the property.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>An action result with the images associated with the property.</returns>
    /// <response code="200">Successfully retrieved images for the property.</response>
    /// <response code="404">The property with the specified ID was not found.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpGet("{propertyId}")]
    [ProducesResponseType(typeof(ResponseDto<IEnumerable<PropertyImageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<bool>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPropertyById(Guid propertyId, CancellationToken cancellationToken)
    {
        var res = await propertyImageService
            .GetImagesByPropertyIdAsync(propertyId, cancellationToken);
        return res.ToActionResult();
    }

    /// <summary>
    /// Uploads images associated with a property and returns the result of each upload operation.
    /// </summary>
    /// <param name="loadImageDto">The data transfer object containing image details and property information.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A response indicating the success or failure of the operation, including partial results.</returns>
    /// <response code="200">Successfully uploaded images.</response>
    /// <response code="207">Some images were successfully uploaded, but some failed. The response contains details of each operation.</response>
    /// <response code="400">Bad request if the input is invalid.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<LoadImageDto>), StatusCodes.Status207MultiStatus)]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage([FromForm] LoadImageDto loadImageDto, CancellationToken cancellationToken)
    {
        var res = await propertyImageService
            .UploadImageAsync(loadImageDto, cancellationToken);
        return res.ToActionResult();
    }
}