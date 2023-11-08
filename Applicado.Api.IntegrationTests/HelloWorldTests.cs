using System.Net;
using Applicado.Api.IntegrationTests.Helpers;
using FluentAssertions;
using FluentAssertions.Web;

namespace Applicado.Api.IntegrationTests;

public class HelloWorldTests
{
    [Fact]
    public async void TestRootEndpoint()
    {
        // Arrange
        await using var application = new TestWebApplicationFactory<Program>();
        using var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1");

        // Assert
        response.Should().Be200Ok();

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Hello From Applicado!");
    }
}
