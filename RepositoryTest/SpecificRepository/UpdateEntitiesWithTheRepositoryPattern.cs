using Context.Data;
using Context.Entities;
using DataTransferObjets.Configuration;
using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repository.GenericRepository.Implentations;
using Repository.GenericRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTest.SpecificRepository
{
    public class UpdateEntitiesWithTheRepositoryPattern
    {
        private DbContextOptions<ApplicationDbContext> DbContextOptions;
        public UpdateEntitiesWithTheRepositoryPattern()
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
        public async Task Can_Update_Owner()
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
            owner.Name = "Updated Name";
            await unitOfWork.OwnerRepository.Update(owner.IdOwner, owner, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            var updatedOwner = await unitOfWork.OwnerRepository.ReadById(CancellationToken.None, o => o.IdOwner == owner.IdOwner);
            Assert.Equal("Updated Name", updatedOwner.Name);
        }

        [Fact]
        public async Task Can_Update_Property_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Original Property",
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
            property.Name = "Updated Property";
            await unitOfWork.PropertyRepository.Update(property.IdProperty, property, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                var updatedProperty = await context.Properties.FindAsync(property.IdProperty);
                Assert.Equal("Updated Property", updatedProperty.Name);
            }
        }

        [Fact]
        public async Task Can_Update_PropertyImage_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "original_image.jpg",
                Img = null,
                Enabled = true,
                IdProperty = Guid.NewGuid() // Asegúrate de que la propiedad exista
            };

            await unitOfWork.PropertyImageRepository.Create(propertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            propertyImage.File = "updated_image.jpg";
            await unitOfWork.PropertyImageRepository.Update(propertyImage.IdPropertyImage, propertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Assert
            using (var context = new ApplicationDbContext(DbContextOptions))
            {
                var updatedImage = await context.PropertyImages.FindAsync(propertyImage.IdPropertyImage);
                Assert.Equal("updated_image.jpg", updatedImage.File);
            }
        }


    }
}
