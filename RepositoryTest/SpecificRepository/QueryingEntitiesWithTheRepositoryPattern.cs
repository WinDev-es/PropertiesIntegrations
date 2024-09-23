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
    public class QueryingEntitiesWithTheRepositoryPattern
    {

        private DbContextOptions<ApplicationDbContext> DbContextOptions;

        public QueryingEntitiesWithTheRepositoryPattern()
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
        public async Task Can_Read_All_Owners_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var owner1 = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = "Owner 1",
                Address = "Address 1",
                Birthday = new DateTime(1980, 1, 1)
            };
            var owner2 = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = "Owner 2",
                Address = "Address 2",
                Birthday = new DateTime(1990, 1, 1)
            };

            await unitOfWork.OwnerRepository.Create(owner1, CancellationToken.None);
            await unitOfWork.OwnerRepository.Create(owner2, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var owners = await unitOfWork.OwnerRepository.ReadAll(CancellationToken.None);

            // Assert
            Assert.Equal(2, owners.Count());
        }

        [Fact]
        public async Task Can_Read_Owner_By_Id_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var owner = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = "Owner for ID",
                Address = "Address ID",
                Birthday = new DateTime(1980, 1, 1)
            };

            await unitOfWork.OwnerRepository.Create(owner, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var retrievedOwner = await unitOfWork.OwnerRepository.ReadById(CancellationToken.None, o => o.IdOwner == owner.IdOwner);

            // Assert
            Assert.NotNull(retrievedOwner);
            Assert.Equal(owner.Name, retrievedOwner.Name);
        }

        [Fact]
        public async Task Can_Read_All_Properties_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property1 = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property One",
                Address = "1111 First St",
                City = "Miami",
                State = "FL",
                ZipCode = "33101",
                Description = "First property.",
                Price = 450000,
                ImageURL = "http://example.com/first_image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente
            };
            var property2 = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Property Two",
                Address = "2222 Second St",
                City = "Houston",
                State = "TX",
                ZipCode = "77001",
                Description = "Second property.",
                Price = 350000,
                ImageURL = "http://example.com/second_image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente
            };

            // Insertar las propiedades
            await unitOfWork.PropertyRepository.Create(property1, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);
            await unitOfWork.PropertyRepository.Create(property2, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var allProperties = await unitOfWork.PropertyRepository.ReadAll(CancellationToken.None);

            // Assert
            Assert.Equal(2, allProperties.Count());
        }

        [Fact]
        public async Task Can_Read_Property_By_Id_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var property = new Property
            {
                IdProperty = Guid.NewGuid(),
                Name = "Unique Property",
                Address = "321 Unique St",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94101",
                Description = "A unique property.",
                Price = 750000,
                ImageURL = "http://example.com/unique_image.jpg",
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente
            };

            // Insertar la propiedad
            await unitOfWork.PropertyRepository.Create(property, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var readProperty = await unitOfWork.PropertyRepository.ReadById(CancellationToken.None, p => p.IdProperty == property.IdProperty);

            // Assert
            Assert.NotNull(readProperty);
            Assert.Equal("Unique Property", readProperty.Name);
        }

        [Fact]
        public async Task Can_Read_All_PropertyImages_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var propertyImage1 = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "image1.jpg",
                Img = null,
                Enabled = true,
                IdProperty = Guid.NewGuid() // Cambiar por una propiedad existente si es necesario
            };
            var propertyImage2 = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "image2.jpg",
                Img = null,
                Enabled = true,
                IdProperty = Guid.NewGuid() // Cambiar por una propiedad existente si es necesario
            };

            // Insertar las imágenes
            await unitOfWork.PropertyImageRepository.Create(propertyImage1, CancellationToken.None);
            await unitOfWork.PropertyImageRepository.Create(propertyImage2, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var allImages = await unitOfWork.PropertyImageRepository.ReadAll(CancellationToken.None);

            // Assert
            Assert.Equal(2, allImages.Count());
        }

        [Fact]
        public async Task Can_Read_PropertyImage_By_Id_Repository()
        {
            // Arrange
            var unitOfWork = await GetUnitOfWorkAsync();
            var propertyImage = new PropertyImage
            {
                IdPropertyImage = Guid.NewGuid(),
                File = "unique_image.jpg",
                Img = null,
                Enabled = true,
                IdProperty = Guid.NewGuid() // Cambiar por una propiedad existente si es necesario
            };

            // Insertar la imagen
            await unitOfWork.PropertyImageRepository.Create(propertyImage, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var readImage = await unitOfWork.PropertyImageRepository.ReadById(CancellationToken.None, img => img.IdPropertyImage == propertyImage.IdPropertyImage);

            // Assert
            Assert.NotNull(readImage);
            Assert.Equal("unique_image.jpg", readImage.File);
        }

    }
}
