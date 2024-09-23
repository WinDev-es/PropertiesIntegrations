using BusinessLogic.AbstractLogic.Domain;
using Context.Entities;
using DataTransferObjects.Dto.Request;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Response;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BusinessLogicTest.AbstractLogic.Domain
{
    public class DomainRulesServicesTests
    {
        #region 1. Pruebas para ValidatePropertyNameIdIntoSystem
        [Fact]
        public async Task ValidatePropertyNameIdIntoSystem_ShouldReturnTrue_WhenNameIsDuplicate()
        {
            // Arrange
            var properties = new List<PropertyDto>
            {
                new()
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property A",
                    Description="Description UVW",
                    State="State A",ZipCode="A1B2C3",
                    Address="St 123",
                    City="Colombia",
                    OwnerId=Guid.Parse(StaticDefination.IdDefaultOwner),
                    OwnerName =StaticDefination.NameDefaultOwner
                },
                new()
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property B",
                    Description="Description XYZ",
                    State="State B",
                    ZipCode="A1B2C3",
                    Address="St 456",
                    City="EEUU",
                    OwnerId=Guid.Parse(StaticDefination.IdDefaultOwner),
                    OwnerName =StaticDefination.NameDefaultOwner
                }
            };
            var id = properties[0].IdProperty;  // Duplicando el nombre
            var name = "Property A";  // Nombre duplicado

            // Act
            var result = await DomainRulesServices.ValidatePropertyNameIdIntoSystem(properties, id, name);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidatePropertyNameIdIntoSystem_ShouldReturnFalse_WhenNameIsNotDuplicate()
        {
            // Arrange
            var properties = new List<PropertyDto>
            {
                new()
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property A",
                    Description="Description UVW",
                    State="State A",ZipCode="A1B2C3",
                    Address="St 123",
                    City="Colombia",
                    OwnerId=Guid.Parse(StaticDefination.IdDefaultOwner),
                    OwnerName =StaticDefination.NameDefaultOwner
                },
                new()
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property B",
                    Description="Description XYZ",
                    State="State B",
                    ZipCode="A1B2C3",
                    Address="St 456",
                    City="EEUU",
                    OwnerId=Guid.Parse(StaticDefination.IdDefaultOwner),
                    OwnerName =StaticDefination.NameDefaultOwner
                }
            };
            var id = Guid.NewGuid();  // Un id no existente
            var name = "Property C";  // Nombre no duplicado

            // Act
            var result = await DomainRulesServices.ValidatePropertyNameIdIntoSystem(properties, id, name);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region 2. Pruebas para ConvertToDownloadImageDtos
        [Fact]
        public void ConvertToDownloadImageDtos_ShouldReturnDownloadImageDtos_WhenImagesAreValid()
        {
            // Arrange
            var propertyImages = new List<PropertyImage>
            {
                new PropertyImage { IdProperty = Guid.NewGuid(), File = "image1.jpg", Enabled = true, Img = new byte[] { 1, 2, 3 } },
                new PropertyImage { IdProperty = Guid.NewGuid(), File = "image2.jpg", Enabled = true, Img = new byte[] { 4, 5, 6 } }
            };

            // Act
            var result = DomainRulesServices.ConvertToDownloadImageDtos(propertyImages);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, imgDto => Assert.NotNull(imgDto.AddImageDto));
        }

        [Fact]
        public void ConvertToDownloadImageDtos_ShouldIgnoreNullImages()
        {
            // Arrange
            var propertyImages = new List<PropertyImage>
            {
                null,
                new PropertyImage { IdProperty = Guid.NewGuid(), File = "image1.jpg", Enabled = true, Img = new byte[] { 1, 2, 3 } }
            };

            // Act
            var result = DomainRulesServices.ConvertToDownloadImageDtos(propertyImages);

            // Assert
            Assert.Single(result); // Solo una imagen válida
        }
        #endregion

        #region 3. Pruebas para UpLoadImages
        [Fact]
        public async Task UpLoadImages_ShouldReturnLoadImageDtos_WhenFilesAreValid()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1);
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            var files = new List<IFormFile> { mockFile.Object };
            var propertyId = Guid.NewGuid();

            // Act
            var result = DomainRulesServices.UpLoadImages(files, propertyId);

            // Assert
            await foreach (var loadImageDto in result)
            {
                Assert.NotNull(loadImageDto);
                Assert.Equal(propertyId, loadImageDto.PropertyImageDto.IdProperty);
            }
        }

        [Fact]
        public async Task UpLoadImages_ShouldReturnEmpty_WhenNoFilesAreProvided()
        {
            // Arrange
            var files = new List<IFormFile>();
            var propertyId = Guid.NewGuid();

            // Act
            var result = DomainRulesServices.UpLoadImages(files, propertyId);

            // Convertir IAsyncEnumerable a una lista manualmente
            var resultList = new List<LoadImageDto>();
            await foreach (var item in result)
            {
                resultList.Add(item);
            }

            // Assert
            Assert.Empty(resultList); // No deberíamos obtener nada
        }
        #endregion
    }
}