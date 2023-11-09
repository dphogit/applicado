using Applicado.Api.Data;
using Applicado.Api.IntegrationTests.Helpers;
using Applicado.Api.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Applicado.Api.IntegrationTests;

public class JobApplicationEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> Factory;

    public JobApplicationEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Factory = factory;
    }

    private void InsertJobApps(IEnumerable<JobApplication> jobApps)
    {
        using var scope = Factory.Services.CreateScope();
        var provider = scope.ServiceProvider;

        using var context = provider.GetRequiredService<DataContext>();
        context.JobApplications.AddRangeAsync(jobApps);
        context.SaveChanges();
    }

    [Fact]
    public async Task Get_ReturnsAllJobApplications()
    {
        JobApplication jobApp1 = new()
        {
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
        };

        JobApplication jobApp2 = new()
        {
            Company = "Google",
            Role = "People and Culture Manager",
            Status = Status.Closed,
        };

        // Arrange
        InsertJobApps(new[] { jobApp1, jobApp2 });
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("job-applications");

        // Assert
        response.Should().Be200Ok().And.BeAs(new[] { jobApp1, jobApp2 });
    }
}
