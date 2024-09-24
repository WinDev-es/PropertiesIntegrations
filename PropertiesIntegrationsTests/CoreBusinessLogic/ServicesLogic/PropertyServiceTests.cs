using AutoMapper;
using BusinessLogic.ServicesLogic;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto.Request;
using DataTransferObjets.Dto.Response;
using DataTransferObjets.Entities;
using Moq;
using Repository.GenericRepository.Interfaces;
using System.Linq.Expressions;

namespace PropertiesIntegrationsTests.CoreBusinessLogic.ServicesLogic
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

        // Caso 1: Crear propiedad - Caso exitoso
        [Fact]
        public async Task CreatePropertyAsync_ShouldReturn201Created_WhenPropertyIsCreatedSuccessfully()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                Name = "Test Property",
                Address = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Description = "A test property",
                Price = 1000
            };

            var propertyEntity = new Property { IdProperty = Guid.NewGuid(), Name = createDto.Name };

            var cancellationToken = new CancellationToken();

            MapperMock.Setup(m => m.Map<Property>(createDto)).Returns(propertyEntity);
            UnitOfWorkMock.Setup(u => u.PropertyRepository.Create(propertyEntity, cancellationToken)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

            // Act
            var result = await PropertyService.CreatePropertyAsync(createDto, cancellationToken);

            // Assert
            Assert.Equal(201, Convert.ToInt16(result.StatusCode));
            Assert.True(result.Data);
        }

        // Caso 2: Crear propiedad - Conflicto de nombre
        // Caso 3: Obtener propiedad por ID - Caso exitoso
        [Fact]
        public async Task GetPropertyByIdAsync_ShouldReturn200OK_WhenPropertyIsFound()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var propertyEntity = new Property { IdProperty = propertyId, Name = "Test Property" };
            var propertyDto = new PropertyDto
            {
                IdProperty = propertyId,
                Name = "Test Property",
                Address = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Description = "A test property",
                Price = 1000,
                OwnerId = Guid.Parse(StaticDefination.IdDefaultOwner),
                OwnerName = StaticDefination.NameDefaultOwner
            };

            var cancellationToken = new CancellationToken();

            UnitOfWorkMock.Setup(u => u.PropertyRepository.ReadById(cancellationToken, It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<string>())).ReturnsAsync(propertyEntity);
            MapperMock.Setup(m => m.Map<PropertyDto>(propertyEntity)).Returns(propertyDto);

            // Act
            var result = await PropertyService.GetPropertyByIdAsync(propertyId, cancellationToken);

            // Assert
            Assert.Equal(200, Convert.ToInt16(result.StatusCode));
            Assert.Equal(propertyDto, result.Data);
        }

        // Caso 4: Obtener propiedad por ID - No encontrada
        [Fact]
        public async Task GetPropertyByIdAsync_ShouldReturn404NotFound_WhenPropertyIsNotFound()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            UnitOfWorkMock.Setup(u => u.PropertyRepository.ReadById(cancellationToken, It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<string>())).ReturnsAsync((Property?)null);

            // Act
            var result = await PropertyService.GetPropertyByIdAsync(propertyId, cancellationToken);

            // Assert
            Assert.Equal(404, Convert.ToInt16(result.StatusCode));
            Assert.Null(result.Data);
        }

        // Caso 5: Actualizar propiedad - Caso exitoso
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
                Price = 2000,
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                Owner = new Owner
                {
                    IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                    Name = StaticDefination.NameDefaultOwner,
                    Address = StaticDefination.AddressDefaultOwner
                }
            };

            var existingProperties = new List<Property>
            {
                new Property
                {
                    IdProperty = propertyId,
                    Name = "Existing Property",
                    Address = "Existing Address",
                    City = "Existing City",
                    State = "Existing State",
                    ZipCode = "12345",
                    Description = "Existing Description",
                    Price = 1500,
                    IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                    Owner = new Owner
                    {
                        IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                        Name = StaticDefination.NameDefaultOwner,
                        Address = StaticDefination.AddressDefaultOwner
                    }
                }
            };

            var cancellationToken = new CancellationToken();

            // Simula el mapeo del DTO a la entidad actualizada
            MapperMock
                .Setup(m => m.Map<Property>(propertyDto))
                .Returns(updatedEntity);

            // Simula la consulta de propiedades existentes
            UnitOfWorkMock
                .Setup(u => u.PropertyRepository.ReadAll(
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Property, bool>>>(),
                    It.IsAny<Func<IQueryable<Property>, IOrderedQueryable<Property>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(existingProperties);

            // Simula la actualización de la propiedad en la base de datos
            UnitOfWorkMock
                .Setup(u => u.PropertyRepository.Update(propertyId, updatedEntity, cancellationToken))
                .Returns(Task.CompletedTask);

            // Simula que se guardó correctamente
            UnitOfWorkMock
                .Setup(u => u.SaveChangesAsync(cancellationToken))
                .ReturnsAsync(1);  // Se simula que 1 cambio fue guardado con éxito

            // Act
            var result = await PropertyService.UpdatePropertyAsync(propertyId, propertyDto, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, Convert.ToInt16(result.StatusCode));  // Verifica que el código de estado sea 200
            Assert.True(result.Data);  // Verifica que la propiedad fue actualizada
        }

        // Caso 6: Actualizar propiedad - Conflicto de nombre
        // Caso 7: Cambiar precio - Caso exitoso
        [Fact]
        public async Task ChangePriceAsync_ShouldReturn204NoContent_WhenPriceIsChangedSuccessfully()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var newPrice = 3000m;

            var propertyEntity = new Property { IdProperty = propertyId, Price = 1500m };

            var cancellationToken = new CancellationToken();

            UnitOfWorkMock.Setup(u => u.PropertyRepository.ReadById(cancellationToken, It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<string>())).ReturnsAsync(propertyEntity);
            UnitOfWorkMock.Setup(u => u.PropertyRepository.Update(propertyId, propertyEntity, cancellationToken)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

            // Act
            var result = await PropertyService.ChangePriceAsync(propertyId, new ChangePriceDto { NewPrice = newPrice }, cancellationToken);

            // Assert
            Assert.Equal(204, Convert.ToInt16(result.StatusCode));
        }

        // Caso 8: Cambiar precio - Precio inválido
        // Caso 9: Eliminar propiedad - Caso exitoso
        [Fact]
        public async Task Delete_ShouldReturn200OK_WhenPropertyIsDeletedSuccessfully()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            UnitOfWorkMock.Setup(u => u.PropertyRepository.Delete(propertyId, cancellationToken)).Returns(Task.CompletedTask);
            UnitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

            // Act
            var result = await PropertyService.DeleteAsync(propertyId, cancellationToken);

            // Assert
            Assert.Equal(200, Convert.ToInt16(result.StatusCode));
            Assert.True(result.Data);
        }
    }
}
