using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using BusinessLogic.Contracts;
using DataTransferObjets.Dto.Response;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Request;
using System.Net;

public class PropertiesControllerTests
{
    private readonly Mock<IPropertyService> PropertyService;
    private readonly PropertiesController PropertiesController;
    public PropertiesControllerTests()
    {
        PropertyService = new Mock<IPropertyService>();
        PropertiesController = new PropertiesController(PropertyService.Object);
    }

    #region 1. ListProperties
    [Fact]
    public async Task ListProperties_ReturnsFilteredPropertyList_WhenValidRequest()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            City = "City 1",
            MinPrice = 50000,
            MaxPrice = 150000
        };

        var properties = new List<PropertyDto>
        {
            new PropertyDto
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property 1",
                Address = "Address 1",
                City = "City 1",
                State = "State 1",
                ZipCode = "Zip 1", Description = "Desc 1",
                Price = 100000,
                ImageURL = "http://test.com/image1.jpg",
                OwnerId = Guid.Parse(StaticDefination.IdDefaultOwner),
                OwnerName = StaticDefination.NameDefaultOwner
            },
            new PropertyDto
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property 2",
                Address = "Address 2",
                City = "City 2",
                State = "State 2",
                ZipCode = "Zip 2",
                Description = "Desc 2",
                Price = 200000,
                ImageURL = "http://test.com/image2.jpg",
                OwnerId = Guid.Parse(StaticDefination.IdDefaultOwner),
                OwnerName = StaticDefination.NameDefaultOwner
            }
        };

        PropertyService
            .Setup(service => service.ListPropertiesAsync(It.IsAny<PropertyFilterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResponseDto<IEnumerable<PropertyDto>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = properties.Where(p => p.City == filter.City && p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice).ToList(),
                Message = "Success"
            });


        // Act
        var result = await PropertiesController.ListProperties(filter, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProperties = Assert.IsType<ResponseDto<List<PropertyDto>>>(okResult.Value);
        Assert.Single(returnedProperties.Data);
        Assert.Equal("Property 1", returnedProperties.Data[0].Name);
    }
    #endregion

    #region 2. CreateProperty
    [Fact]
    public async Task CreateProperty_ReturnsCreated_WhenValidProperty()
    {
        // Arrange
        var newProperty = new CreatePropertyDto
        {
            Name = "New Property",
            Address = "New Address",
            City = "New City",
            State = "New State",
            ZipCode = "New Zip",
            Description = "New Description",
            Price = 150000
        };

        // Mockeamos la respuesta del servicio
        PropertyService
            .Setup(service => service.CreatePropertyAsync(newProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.CreateResponse201(true)); // Aquí devolvemos un ResponseDto<bool> indicando éxito

        // Act
        var result = await PropertiesController.CreateProperty(newProperty, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var response = Assert.IsType<ResponseDto<bool>>(createdResult.Value); // Espera un ResponseDto<bool>
        Assert.True(response.Data); // Verifica que la propiedad fue creada correctamente
        Assert.Equal(HttpStatusCode.Created, response.StatusCode); // Verifica que el estado sea 201
    }

    [Fact]
    public async Task CreateProperty_ReturnsBadRequest_WhenPropertyIsInvalid()
    {
        // Arrange
        var invalidProperty = new CreatePropertyDto
        {
            Name = null, // Invalid: required field
            Address = "Invalid Address",
            City = "Invalid City",
            State = "Invalid State",
            ZipCode = "Invalid Zip",
            Description = "Invalid Description",
            Price = 150000
        };

        // Act
        var result = await PropertiesController.CreateProperty(invalidProperty, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    #endregion

    #region 3. UpdatePropertyAsync
    [Fact]
    public async Task UpdatePropertyAsync_ReturnsOk_WhenPropertyExists()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var updatedProperty = new UpdatePropertyDto
        {
            Name = "Updated Property",
            Address = "Updated Address",
            City = "Updated City",
            State = "Updated State",
            ZipCode = "Updated Zip",
            Description = "Updated Description",
            Price = 120000
        };

        PropertyService
            .Setup(service => service.UpdatePropertyAsync(propertyId, updatedProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.SuccessfulResponse200(true));

        // Act
        var result = await PropertiesController.UpdatePropertyAsync(propertyId, updatedProperty, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task UpdatePropertyAsync_ReturnsNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var updatedProperty = new UpdatePropertyDto
        {
            Name = "Updated Property",
            Address = "Updated Address",
            City = "Updated City",
            State = "Updated State",
            ZipCode = "Updated Zip",
            Description = "Updated Description",
            Price = 120000
        };

        PropertyService
            .Setup(service => service.UpdatePropertyAsync(propertyId, updatedProperty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.NotFound404<bool>("Property not found"));

        // Act
        var result = await PropertiesController.UpdatePropertyAsync(propertyId, updatedProperty, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    #endregion

    #region 4. ChangePrice

    [Fact]
    public async Task ChangePrice_ReturnsOk_WhenPriceIsUpdated()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var changePriceDto = new ChangePriceDto
        {
            NewPrice = 130000
        };

        PropertyService
            .Setup(service => service.ChangePriceAsync(propertyId, changePriceDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.SuccessfulResponse200(true));

        // Act
        var result = await PropertiesController.ChangePrice(propertyId, changePriceDto, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task ChangePrice_ReturnsNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var changePriceDto = new ChangePriceDto
        {
            NewPrice = 130000
        };

        PropertyService
            .Setup(service => service.ChangePriceAsync(propertyId, changePriceDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.NotFound404<bool>("Property not found"));

        // Act
        var result = await PropertiesController.ChangePrice(propertyId, changePriceDto, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    #endregion

    #region 5. Get Property by Id
    [Fact]
    public async Task GetPropertyById_ReturnsOk_WhenPropertyFound()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var expectedProperty = new PropertyDto
        {
            IdProperty = propertyId,
            Name = "Test Property",
            Address = "123 Test St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Description = "A test property",
            Price = 100000,
            ImageURL = "http://test.com/image.jpg",
            OwnerId = Guid.Parse(StaticDefination.IdDefaultOwner),
            OwnerName = StaticDefination.NameDefaultOwner
        };

        PropertyService.Setup(service => service.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.SuccessfulResponse200(expectedProperty));

        // Act
        var result = await PropertiesController.GetPropertyById(propertyId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto<PropertyDto>>(okResult.Value);
        Assert.Equal(expectedProperty, response.Data);
    }

    [Fact]
    public async Task GetPropertyById_ReturnsNotFound_WhenPropertyNotFound()
    {
        // Arrange
        var propertyId = Guid.NewGuid();

        PropertyService.Setup(service => service.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResponses.NotFound404<PropertyDto>("Property not found"));

        // Act
        var result = await PropertiesController.GetPropertyById(propertyId, CancellationToken.None);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseDto<string>>(notFoundResult.Value);
        Assert.Equal("Property not found", response.Message);
    }
    #endregion
}
