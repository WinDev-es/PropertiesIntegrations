using AutoMapper;
using BusinessLogic.ServicesLogic;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Entities;
using Moq;
using Repository.GenericRepository.Interfaces;
using System.Linq.Expressions;

namespace BusinessLogicTest.ServicesLogic
{
    public class PropertyServiceTests
    {
        private readonly Mock<IMapper> MapperMock;
        private readonly Mock<IUnitOfWork> UnitOfWorkMock;
        private readonly PropertyService PropertyService;

        public PropertyServiceTests()
        {
            MapperMock = new Mock<IMapper>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            PropertyService = new PropertyService(MapperMock.Object, UnitOfWorkMock.Object);
        }
        [Fact]
        public async Task UpdatePropertyAsync_ShouldReturn200OK_WhenUpdateIsSuccessful()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var propertyDto = new UpdatePropertyDto
            {
                Name = "Updated Property",
                Address = "Updated Address",
                City = "Updated City",
                State = "Updated State",
                ZipCode = "54321",
                Description = "Updated Description",
                Price = 2000
            };

            var updatedEntity = new Property
            {
                IdProperty = propertyId,
                Name = "Updated Property",
                Address = "Updated Address",
                City = "Updated City",
                State = "Updated State",
                ZipCode = "54321",
                Description = "Updated Description",
                Price = 2000
            };

            var cancellationToken = new CancellationToken();

            // Mapea el DTO a la entidad actualizada
            MapperMock
                .Setup(m => m.Map<Property>(propertyDto))
                .Returns(updatedEntity);

            // Simula que no hay conflictos de nombres
            UnitOfWorkMock
                .Setup(u => u.PropertyRepository.ReadAll(
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Property, bool>>>(),
                    It.IsAny<Func<IQueryable<Property>, IOrderedQueryable<Property>>>(),
                    It.IsAny<string>())
                )
                .ReturnsAsync(new List<Property>());

            // Simula la actualización de la propiedad en la base de datos
            UnitOfWorkMock
                .Setup(u => u.PropertyRepository.Update(propertyId, updatedEntity, cancellationToken))
                .Returns(Task.CompletedTask);

            UnitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);  // Simulamos que se guardó con éxito

            // Act
            var result = await PropertyService.UpdatePropertyAsync(propertyId, propertyDto, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, Convert.ToInt16(result.StatusCode));  // Verifica que el código de estado sea 200
            Assert.True(result.Data);  // Verifica que la propiedad fue actualizada
        }
    }
}
