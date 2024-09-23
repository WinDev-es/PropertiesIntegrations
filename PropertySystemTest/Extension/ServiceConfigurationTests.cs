using BusinessLogic.Contracts;
using Context.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class ServiceConfigurationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ServiceConfigurationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void ConfigureServices_RegistersServices()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var serviceProvider = _factory.Services;

        // Assert
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;

            // Verifica que el servicio IPropertyService esté registrado
            var propertyService = services.GetService<IPropertyService>();
            Assert.NotNull(propertyService);

            // Verifica que el servicio IPropertyImageService esté registrado
            var propertyImageService = services.GetService<IPropertyImageService>();
            Assert.NotNull(propertyImageService);

            // Verifica que el DbContext esté registrado
            var dbContext = services.GetService<ApplicationDbContext>();
            Assert.NotNull(dbContext);
        }
    }
}
