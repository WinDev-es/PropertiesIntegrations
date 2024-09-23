using BusinessLogic.Contracts;
using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace PropertySystemTest.Controllers
{
    public class PropertyImagesControllerTests
    {

        private readonly Mock<IPropertyImageService> PropertyImagesService;
        private readonly PropertyImagesController PropertyImagesController;
        public PropertyImagesControllerTests()
        {
            PropertyImagesService = new Mock<IPropertyImageService>();
            PropertyImagesController = new PropertyImagesController(PropertyImagesService.Object);
        }

        #region GetPropertyById Tests
        [Fact]
        public async Task GetPropertyById_ReturnsOk_WhenImagesExist()
        {
            // Arrange
            var propertyId = Guid.NewGuid();

            var downloadImageDtoList = new List<DownloadImageDto>
    {
        new DownloadImageDto
        {
            AddImageDto = new AddImageDto
            {
                ImageUrl = "http://example.com/image1.jpg",
                Img = null,
                Files = new List<IFormFile>()
            },
            PropertyImageDto = new PropertyImageDto
            {
                File = "image1.jpg",
                Enabled = true,
                IdProperty = propertyId
            }
        },
        new DownloadImageDto
        {
            AddImageDto = new AddImageDto
            {
                ImageUrl = "http://example.com/image2.jpg",
                Img = null,
                Files = new List<IFormFile>()
            },
            PropertyImageDto = new PropertyImageDto
            {
                File = "image2.jpg",
                Enabled = true,
                IdProperty = propertyId
            }
        }
    };

            var response = ServiceResponses.SuccessfulResponse200<IEnumerable<DownloadImageDto>>(downloadImageDtoList);

            // Configuración del mock
            PropertyImagesService
                .Setup(service => service.GetImagesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await PropertyImagesController.GetPropertyById(propertyId, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponse = Assert.IsType<ResponseDto<IEnumerable<DownloadImageDto>>>(okResult.Value);

            Assert.Equal(downloadImageDtoList.Count, returnedResponse.Data.Count());
            Assert.Equal(downloadImageDtoList.First().PropertyImageDto.File, returnedResponse.Data.First().PropertyImageDto.File);
        }

        [Fact]
        public async Task GetPropertyById_ReturnsNotFound_WhenNoImagesExist()
        {
            // Arrange
            var propertyId = Guid.NewGuid();

            PropertyImagesService
                .Setup(service => service.GetImagesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.NotFound404<IEnumerable<DownloadImageDto>>("Property not found"));

            // Act
            var result = await PropertyImagesController.GetPropertyById(propertyId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ResponseDto<string>>(notFoundResult.Value);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Property not found", response.Message);
        }


        #endregion

        #region UploadImage Tests
        [Fact]
        public async Task UploadImage_ReturnsOk_WhenUploadSuccessful()
        {
            // Arrange
            var loadImageDto = new LoadImageDto
            {
                AddImageDto = new AddImageDto { ImageUrl = "image.jpg", Files = new List<IFormFile>() },
                PropertyImageDto = new PropertyImageDto { File = "image.jpg", Enabled = true, IdProperty = Guid.NewGuid() }
            };

            PropertyImagesService
                .Setup(service => service.UploadImageAsync(loadImageDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.SuccessfulResponse200(true)); // La carga fue exitosa

            // Act
            var result = await PropertyImagesController.UploadImage(loadImageDto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto<bool>>(okResult.Value);
            Assert.True(response.Data); // Verifica que la carga fue exitosa
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UploadImage_ReturnsMultiStatus_WhenPartialSuccess()
        {
            // Arrange
            var loadImageDto = new LoadImageDto
            {
                AddImageDto = new AddImageDto { ImageUrl = "image.jpg", Files = new List<IFormFile>() },
                PropertyImageDto = new PropertyImageDto { File = "image.jpg", Enabled = true, IdProperty = Guid.NewGuid() }
            };

            PropertyImagesService
                .Setup(service => service.UploadImageAsync(loadImageDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.MultiStatus207<bool>("Some images failed to upload"));

            // Act
            var result = await PropertyImagesController.UploadImage(loadImageDto, CancellationToken.None);

            // Assert
            var multiStatusResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsType<ResponseDto<string>>(multiStatusResult.Value);
            Assert.Equal(HttpStatusCode.MultiStatus, response.StatusCode);
            Assert.Equal("Some images failed to upload", response.Message);
        }

        [Fact]
        public async Task UploadImage_ReturnsBadRequest_WhenInvalidInput()
        {
            // Arrange
            var loadImageDto = new LoadImageDto(); // Input inválido (puedes personalizar esto según tus validaciones)

            PropertyImagesService
                .Setup(service => service.UploadImageAsync(loadImageDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResponses.BadRequestResponse400<bool>("Invalid input"));

            // Act
            var result = await PropertyImagesController.UploadImage(loadImageDto, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseDto<string>>(badRequestResult.Value);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Invalid input", response.Message);
        }

        #endregion
    }
}
