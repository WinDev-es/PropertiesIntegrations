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
namespace PropertySystemTest.Controllers
{
    public class PropertiesControllerTests
    {
        private readonly Mock<IPropertyService> PropertyServiceMock;
        private readonly PropertiesController PropertiesController;
        public PropertiesControllerTests()
        {
            PropertyServiceMock = new Mock<IPropertyService>();
            PropertiesController = new PropertiesController(PropertyServiceMock.Object);
        }

        // Método para limpiar recursos después de cada prueba
        public void Dispose()
        {
            // Limpia y reinicia los mocks
            PropertyServiceMock.Reset();
        }

        #region 1. ListProperties

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
            PropertyServiceMock
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

            PropertyServiceMock
                .Setup(service => service.UpdatePropertyAsync(propertyId, updatedProperty, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.SuccessfulResponse200(true));

            // Act
            var result = await PropertiesController.UpdatePropertyAsync(propertyId, updatedProperty, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto<bool>>(okResult.Value);
            Assert.True(response.Data);
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

            PropertyServiceMock
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

            // Configura el mock para devolver un ResponseDto indicando que no se encontró la propiedad
            PropertyServiceMock
                .Setup(service => service.ChangePriceAsync(propertyId, changePriceDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.NotFound404<bool>("Property not found"));

            // Act
            var result = await PropertiesController.ChangePrice(propertyId, changePriceDto, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto<bool>>(notFoundResult.Value);
            Assert.Equal("Property not found", responseDto.Message);
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

            PropertyServiceMock.Setup(service => service.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.SuccessfulResponse200(expectedProperty));

            // Act
            var result = await PropertiesController.GetPropertyById(propertyId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto<PropertyDto>>(okResult.Value);
            Assert.Equal(expectedProperty, response.Data);
        }
        #endregion
    }
}