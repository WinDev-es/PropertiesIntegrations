using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PropertySystemTest.Middleware
{
    public class ExceptionHandlingMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExceptionHandlingMiddlewareTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Middleware_Returns500_WhenExceptionThrown()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/throw-exception"); // Reemplaza con un endpoint que lanza una excepción.

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonErrorResponse>(responseBody);

            Assert.NotNull(jsonResponse);
            Assert.Equal("An unexpected error occurred.", jsonResponse.Error);
        }

        private class JsonErrorResponse
        {
            public string Error { get; set; }
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("throw-exception")]
        public IActionResult ThrowException()
        {
            throw new Exception("This is a test exception");
        }
    }
}
