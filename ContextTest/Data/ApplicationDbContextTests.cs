using Context.Data;
using Context.Entities;
using DataTransferObjets.Configuration;
using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextTest.Data
{
    public class ApplicationDbContextTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ApplicationDbContextTests()
        {
            // Configurar el DbContext para usar una base de datos en memoria
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }
        private void AddDefaulOwnerAsync()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var owner = new Owner
                {
                    IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                    Name = StaticDefination.NameDefaultOwner,
                    Address = StaticDefination.AddressDefaultOwner
                };

                // Act
                context.Owners.Add(owner);
                context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task Can_Insert_Owner_Into_Database()
        {
            // Arrange
            AddDefaulOwnerAsync();

            // Assert
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                Assert.Equal(2, await context.Owners.CountAsync());
                var ownerFromDb = await context.Owners.FirstOrDefaultAsync();
                Assert.Equal(StaticDefination.NameDefaultOwner, ownerFromDb.Name);
            }
        }

        [Fact]
        public async Task Can_Insert_Property_Into_Database()
        {
            // Arrange
            AddDefaulOwnerAsync();
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
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
                    IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner) // Asegúrate de tener un owner existente o usar un Guid válido
                };

                // Act
                context.Properties.Add(property);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                Assert.Equal(1, await context.Properties.CountAsync());
                var propertyFromDb = await context.Properties.FirstOrDefaultAsync();
                Assert.Equal("Luxury Apartment", propertyFromDb.Name);
                Assert.Equal("123 Main St", propertyFromDb.Address);
            }
        }

        [Fact]
        public async Task Can_Insert_PropertyImage_Into_Database()
        {
            // Arrange
            AddDefaulOwnerAsync();
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                // Asegúrate de que la propiedad a la que se vinculará la imagen ya esté en la base de datos
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

                context.Properties.Add(property);
                await context.SaveChangesAsync();

                var propertyImage = new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    File = "property_image.jpg",
                    Img = null, // Simulando que no hay imagen
                    Enabled = true,
                    IdProperty = property.IdProperty // Asociamos la imagen con la propiedad
                };

                // Act
                context.PropertyImages.Add(propertyImage);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                Assert.Equal(1, await context.PropertyImages.CountAsync());
                var imageFromDb = await context.PropertyImages.FirstOrDefaultAsync();
                Assert.Equal("property_image.jpg", imageFromDb.File);
                Assert.True(imageFromDb.Enabled);
            }
        }


    }
}
