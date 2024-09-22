using Context.Data;
using Context.Entities;
using DataTransferObjets.Configuration;
using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.GenericRepository.Implentations;
using Repository.GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTest.SpecificRepository
{
    public class AddingEntitiesWithTheRepositoryPattern
    {
        private DbContextOptions<ApplicationDbContext> DbContextOptions;

        public AddingEntitiesWithTheRepositoryPattern()
        {
            // Configurar el DbContext para usar una base de datos en memoria
            DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private async Task<IUnitOfWork> GetUnitOfWorkAsync()
        {
            var context = new ApplicationDbContext(DbContextOptions);
            return new UnitOfWork(context);
        }

        [Fact]
        public async Task Can_Add_Owner_Repository()
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

            // Act
            await unitOfWork.OwnerRepository.Create(owner, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(1, await context.Owners.CountAsync());
                var ownerFromDb = await context.Owners.FirstOrDefaultAsync();
                Assert.Equal(StaticDefination.NameDefaultOwner, ownerFromDb.Name);
            }
        }

        [Fact]
        public async Task Can_Add_Property_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Luxury Apartment",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "10001",
                Description = "A beautiful apartment with a view.",
                Price = 500000,
                ImageURL = "http://example.com/image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente o usa un Guid válido
            };

            // Act
            await unitOfWork.PropertyRepository.Create(property, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(1, await context.Properties.CountAsync());
                var propertyFromDb = await context.Properties.FirstOrDefaultAsync();
                Assert.Equal("Luxury Apartment", propertyFromDb.Name);
            }
        }

        [Fact]
        public async Task Can_Add_PropertyImage_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Luxury Apartment",
                Address = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "10001",
                Description = "A beautiful apartment with a view.",
                Price = 500000,
                ImageURL = "http://example.com/image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente
            };

            // Agregar propiedad primero
            await unitOfWork.PropertyRepository.Create(property, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            var propertyImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "property_image.jpg",
                Img = null, // Simulando que no hay imagen
                Enabled = true,
                IdProperty = property.IdProperty // Asociamos la imagen con la propiedad
            };

            // Act
            await unitOfWork.PropertyImageRepository.Create(propertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                Assert.Equal(1, await context.PropertyImages.CountAsync());
                var imageFromDb = await context.PropertyImages.FirstOrDefaultAsync();
                Assert.Equal("property_image.jpg", imageFromDb.File);
                Assert.True(imageFromDb.Enabled);
            }
        }
    }
}
