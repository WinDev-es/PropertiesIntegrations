using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace PropertySystemTest.Extension
{
    public class CorsServicesExtensionTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        public CorsServicesExtensionTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task CORS_Configuration_AllowsAnyOrigin()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/list-properties"); //Reemplaza con un endpoint que lanza un OK.

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
