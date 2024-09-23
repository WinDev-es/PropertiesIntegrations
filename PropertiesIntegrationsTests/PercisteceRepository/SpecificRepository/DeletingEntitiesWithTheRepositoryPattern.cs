using Context.Data;
using Context.Entities;
using DataTransferObjets.Configuration;
using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository.Implentations;
using Repository.GenericRepository.Interfaces;

namespace PropertiesIntegrationsTests.PercisteceRepository.SpecificRepository
{
    public class DeletingEntitiesWithTheRepositoryPattern
    {
        private DbContextOptions<ApplicationDbContext> DbContextOptions;

        public DeletingEntitiesWithTheRepositoryPattern()
        {
            // Configurar el DbContext para usar una base de datos en memoria
            DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task<IUnitOfWork> GetUnitOfWorkAsync()
        {
            var context = new ApplicationDbContext(DbContextOptions);
            return new UnitOfWork(context);
        }

        [Fact]
        public async Task Can_Delete_Owner_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var owner = new Owner
            {
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                Name = StaticDefination.NameDefaultOwner,
                Address = StaticDefination.AddressDefaultOwner,
                Birthday = new DateTime(1980, 1, 1)
            };

            await unitOfWork.OwnerRepository.Create(owner, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            await unitOfWork.OwnerRepository.Delete(owner.IdOwner, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(0, await context.Owners.CountAsync());
            }
        }

        [Fact]
        public async Task Can_Delete_Property_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property to Delete",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "10001",
                Description = "Description",
                Price = 500000,
                ImageURL = "http://example.com/image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner)
            };

            await unitOfWork.PropertyRepository.Create(property, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            await unitOfWork.PropertyRepository.Delete(property.IdProperty, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(0, await context.Properties.CountAsync());
            }
        }

        [Fact]
        public async Task Can_Delete_PropertyImage_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "image_to_delete.jpg",
                Img = null,
                Enabled = true,
                IdProperty = Guid.NewGuid() // Asegúrate de que la propiedad exista
            };

            await unitOfWork.PropertyImageRepository.Create(propertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            await unitOfWork.PropertyImageRepository.Delete(propertyImage.IdPropertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(0, await context.PropertyImages.CountAsync());
            }
        }

    }
}
