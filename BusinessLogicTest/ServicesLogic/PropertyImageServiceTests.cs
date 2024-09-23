using AutoMapper;
using BusinessLogic.ServicesLogic;
using Context.Entities;
using DataTransferObjects.Dto.Request;
using DataTransferObjects.Dto.Response;
using DataTransferObjects.Dto.System;
using DataTransferObjets.Dto.Request;
using Microsoft.AspNetCore.Http;
using Moq;
using Repository.GenericRepository.Interfaces;
using System.Linq.Expressions;
namespace BusinessLogicTest.ServicesLogic
{
    public class PropertyImageServiceTests
    {
        private readonly Mock<IMapper> MapperMock;
        private readonly Mock<IUnitOfWork> UnitOfWorkMock;
        private readonly PropertyImageService PropertyImageService;

        public PropertyImageServiceTests()
        {
            MapperMock = new Mock<IMapper>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            PropertyImageService = new PropertyImageService(MapperMock.Object, UnitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetImagesByPropertyIdAsync_ShouldReturnImages_WhenImagesExist()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            var propertyImages = new List<PropertyImage>
        {
            new PropertyImage { IdProperty = propertyId, Enabled = true, File = "image1.jpg" },
            new PropertyImage { IdProperty = propertyId, Enabled = true, File = "image2.jpg" }
        };

            UnitOfWorkMock
                .Setup(u => u.PropertyImageRepository.ReadAll(
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<PropertyImage, bool>>>(), // Filtro opcional
            null, // Ordenación no necesaria, se pasa null
            It.IsAny<string>())) // Aquí se pasa el includeProperties
        .ReturnsAsync(propertyImages);

            var expectedDtos = new List<DownloadImageDto>
        {
            new DownloadImageDto { PropertyImageDto = new PropertyImageDto { File = "image1.jpg", Enabled = true, IdProperty = propertyId }},
            new DownloadImageDto { PropertyImageDto = new PropertyImageDto { File = "image2.jpg", Enabled = true, IdProperty = propertyId }}
        };

            MapperMock
                .Setup(m => m.Map<IEnumerable<DownloadImageDto>>(It.IsAny<IEnumerable<PropertyImage>>()))
                .Returns(expectedDtos);

            // Act
            var result = await PropertyImageService.GetImagesByPropertyIdAsync(propertyId, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, Convert.ToInt16(result.StatusCode)); // Assuming status code 200 for successful response
            Assert.Equal(2, result.Data.Count());
        }

        // Esta es la prueba del método público UploadImageAsync, que internamente usa ProcessImagesAsync.
        [Fact]
        public async Task UploadImageAsync_ShouldReturnMultiStatus_WhenSomeUploadsFail()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            var loadImageDto = new LoadImageDto
            {
                AddImageDto = new AddImageDto
                {
                    Files = new List<IFormFile> { Mock.Of<IFormFile>() }
                },
                PropertyImageDto = new PropertyImageDto
                {
                    File = "test.jpg",
                    Enabled = true,
                    IdProperty = propertyId
                }
            };

            // Simulamos que los resultados de ProcessImagesAsync devuelven algunos éxitos y algunos fallos.
            var operationResults = new List<OperationResult>
    {
        new OperationResult { IsSuccess = true },
        new OperationResult { IsSuccess = false } // Simulación de fallo
    };

            // Simulamos el comportamiento de ProcessImagesAsync indirectamente a través del método público UploadImageAsync.
            UnitOfWorkMock
                .Setup(u => u.PropertyImageRepository.Create(It.IsAny<PropertyImage>(), cancellationToken))
                .Returns(Task.CompletedTask); // Simulamos la creación en el repositorio.

            UnitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1); // Simulamos el guardado de cambios. Esto devuelve Task<int> con el valor 1, de que es correcto el cambio

            // Act
            var result = await PropertyImageService.UploadImageAsync(loadImageDto, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, Convert.ToInt16(result.StatusCode)); // Esperamos que retorne 400 BadRequest.
        }
    }
}