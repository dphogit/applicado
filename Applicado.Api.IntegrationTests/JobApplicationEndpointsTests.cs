using System.Net.Http.Json;
using Applicado.Api.Data;
using Applicado.Api.Features.JobApplications;
using Applicado.Api.IntegrationTests.Helpers;
using Applicado.Api.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Applicado.Api.IntegrationTests;

public class JobApplicationEndpointsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> Factory;

    public readonly static IEnumerable<object[]> MissingRequiredFieldsPostDtos = new List<object[]>
    {
        new object[] { new { Role = "Software Engineer" } },    // No "Company"
        new object[] { new { Company = "Microsoft" } },         // No "Role"
    };

    public JobApplicationEndpointsTests(TestWebApplicationFactory<Program> factory)
    {
        Factory = factory;
    }

    private async Task<JobApplication?> GetJobApp(Guid id)
    {
        using var scope = Factory.Services.CreateScope();
        var provider = scope.ServiceProvider;

        using var context = provider.GetRequiredService<DataContext>();
        return await context.JobApplications.FindAsync(id);
    }

    private void InsertJobApps(IEnumerable<JobApplication> jobApps)
    {
        using var scope = Factory.Services.CreateScope();
        var provider = scope.ServiceProvider;

        using var context = provider.GetRequiredService<DataContext>();
        context.JobApplications.AddRangeAsync(jobApps);
        context.SaveChanges();
    }

    private void InsertJobApp(JobApplication jobApp)
    {
        InsertJobApps(new[] { jobApp });
    }

    [Fact]
    public async Task Get_ReturnsOkWithAllJobApplications()
    {
        // Arrange
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

        InsertJobApps(new[] { jobApp1, jobApp2 });
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync("job-applications");

        // Assert
        response.Should().Be200Ok().And.BeAs(new[] { jobApp1, jobApp2 });
    }

    [Fact]
    public async Task Get_WithExistingId_ReturnsOkWithJobApplication()
    {
        // Arrange
        JobApplication jobApp = new()
        {
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
        };

        InsertJobApp(jobApp);
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"job-applications/{jobApp.Id}");

        // Assert
        response.Should().Be200Ok().And.BeAs(jobApp);
    }

    [Fact]
    public async Task Get_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var client = Factory.CreateClient();

        // Act
        var response = await client.GetAsync($"job-applications/{id}");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task Post_WithValidJobApp_ReturnsCreatedWithJobApp()
    {
        // Arrange
        CreateJobApplicationDto jobAppDto = new()
        {
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
        };

        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("job-applications", jobAppDto);

        // Assert
        response.Should().Be201Created().And.BeAs(jobAppDto);
    }

    [Theory]
    [MemberData(nameof(MissingRequiredFieldsPostDtos))]
    public async Task Post_WithMissingRequiredField_ReturnsBadRequest(object invalidDto)
    {
        // Arrange
        var client = Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("job-applications", invalidDto);

        // Assert
        response.Should().Be400BadRequest();
    }

    [Fact]
    public async Task Put_WithExistingId_UpdatesAndReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        InsertJobApp(new()
        {
            Id = id,
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
        });

        UpdateJobApplicationDto jobAppDto = new()
        {
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Rejected,
        };

        var client = Factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync($"job-applications/{id}", jobAppDto);

        // Assert
        response.Should().Be204NoContent();

        var updatedJobApp = await GetJobApp(id);
        updatedJobApp.Should().BeEquivalentTo(jobAppDto);
    }

    [Fact]
    public async Task Put_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        UpdateJobApplicationDto jobAppDto = new() { Company = "Microsoft", Role = "Software Engineer" };

        var client = Factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync($"job-applications/{id}", jobAppDto);

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task Delete_WithExisitingId_DeletesAndReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        InsertJobApp(new() { Id = id, Company = "Microsoft", Role = "Software Engineer" });

        var client = Factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"job-applications/{id}");

        // Assert
        response.Should().Be204NoContent();

        var deletedJobApp = await GetJobApp(id);
        deletedJobApp.Should().BeNull();
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var client = Factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"job-applications/{id}");

        // Assert
        response.Should().Be404NotFound();
    }
}
