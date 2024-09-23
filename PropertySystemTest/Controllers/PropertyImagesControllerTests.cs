using BusinessLogic.Contracts;
using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace PropertySystemTest.Controllers
{
    public class PropertyImagesControllerTests : IDisposable
    {
        private readonly Mock<IPropertyImageService> PropertyImagesServiceMock;
        private readonly PropertyImagesController PropertyImagesController;

        public PropertyImagesControllerTests()
        {
            PropertyImagesServiceMock = new Mock<IPropertyImageService>();
            PropertyImagesController = new PropertyImagesController(PropertyImagesServiceMock.Object);
        }

        // Método para limpiar recursos después de cada prueba
        public void Dispose()
        {
            // Limpia y reinicia los mocks
            PropertyImagesServiceMock.Reset();
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
            PropertyImagesServiceMock
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

            // Configuración del Mock para devolver una respuesta de no encontrado
            var response = ServiceResponses.NotFound404<IEnumerable<DownloadImageDto>>(StaticDefination.NoData);
            PropertyImagesServiceMock
                .Setup(service => service.GetImagesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response); // Esto debe devolver el tipo correcto

            // Act
            var result = await PropertyImagesController.GetPropertyById(propertyId, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var responseDto = Assert.IsType<ResponseDto<IEnumerable<DownloadImageDto>>>(notFoundResult.Value);
            Assert.Equal(HttpStatusCode.NotFound, responseDto.StatusCode);
            Assert.Equal(StaticDefination.NoData, responseDto.Message);
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

            PropertyImagesServiceMock
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
        #endregion
    }
}
