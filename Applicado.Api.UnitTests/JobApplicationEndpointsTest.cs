using Applicado.Api.Features.JobApplications;
using Applicado.Api.Models;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Applicado.Api.UnitTests;

public class JobApplicationEndpointsTests
{
    [Fact]
    public async Task GetAll_JobAppsInDb_ReturnsJobApps()
    {
        // Arrange
        List<JobApplication> expectedJobApps = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Microsoft",
                Role = "Software Engineer",
                Status = Status.Applied,
                Link = "https://jobs.microsoft.com/12345",
                CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Google",
                Role = "People and Culture Manager",
                Status = Status.Closed,
                Link = "https://careers.google.com/12345",
                CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
            }
        };

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAllAsync().Result).Returns(expectedJobApps);

        // Act
        var result = await JobApplicationEndpoints.GetAll(mock.Object);

        // Assert
        result.Should().BeOfType<Ok<JobApplicationDto[]>>()
            .Which.Value.Should().HaveCount(2).And.BeEquivalentTo(expectedJobApps);
    }

    [Fact]
    public async Task Get_JobAppInDb_ReturnsJobApp()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var expectedJobApp = new JobApplication
        {
            Id = expectedId,
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
            Link = "https://jobs.microsoft.com/12345",
            CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
        };

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(expectedId).Result).Returns(expectedJobApp);

        // Act
        var result = await JobApplicationEndpoints.Get(mock.Object, expectedJobApp.Id);

        // Assert
        result.Should().BeOfType<Results<Ok<JobApplicationDto>, NotFound>>()
            .Which.Result.Should().BeOfType<Ok<JobApplicationDto>>()
                .Which.Value.Should().BeEquivalentTo(expectedJobApp);
    }

    [Fact]
    public async Task Get_JobAppNotExist_ReturnsNotFound()
    {
        // Arrange
        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(It.IsAny<Guid>()).Result)
            .Returns((JobApplication?)null);

        // Act
        var result = await JobApplicationEndpoints.Get(mock.Object, Guid.NewGuid());

        // Assert
        result.Should().BeOfType<Results<Ok<JobApplicationDto>, NotFound>>()
            .Which.Result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task Create_ValidJobApp_CreatesInDbAndReturnsIt()
    {
        // Arrange
        List<JobApplication> jobs = new();

        var createJobAppDto = new CreateJobApplicationDto
        {
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
            Link = "https://jobs.microsoft.com/12345",
            CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
        };

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.CreateAsync(It.IsAny<JobApplication>()))
            .Callback<JobApplication>(createdJobApp =>
            {
                createdJobApp.Id = Guid.NewGuid();
                jobs.Add(createdJobApp);
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await JobApplicationEndpoints.Create(mock.Object, createJobAppDto);

        // Assert
        result = result.Should().BeOfType<CreatedAtRoute<JobApplicationDto>>().Which;

        result.RouteName.Should().Be("GetJobApplicationById");
        result.RouteValues.Values.Should().Contain(jobs[0].Id.ToString());
        result.Value.Should().BeEquivalentTo(createJobAppDto);

        jobs.Should().HaveCount(1).And.ContainEquivalentOf(createJobAppDto);
    }

    [Fact]
    public async Task Update_ExistingJobApp_UpdatesInDb()
    {
        // Arrange
        var dbJobAppId = Guid.NewGuid();
        var dbJobApp = new JobApplication
        {
            Id = dbJobAppId,
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
            Link = "https://jobs.microsoft.com/12345",
            CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
        };

        var updateJobAppDto = new UpdateJobApplicationDto
        {
            Company = "Google",
            Role = "People and Culture Manager",
            Status = Status.Closed,
            Link = "https://careers.google.com/12345",
            CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
        };

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(It.Is<Guid>(id => id == dbJobAppId)).Result)
            .Returns(dbJobApp);

        mock.Setup(service => service.UpdateAsync(It.IsAny<JobApplication>()))
            .Callback<JobApplication>(updatedJobApp => dbJobApp = updatedJobApp)
            .Returns(Task.CompletedTask);

        // Act
        var result = await JobApplicationEndpoints.Update(mock.Object, dbJobAppId, updateJobAppDto);

        // Assert
        result.Should().BeOfType<Results<NoContent, NotFound>>()
            .Which.Result.Should().BeOfType<NoContent>();

        dbJobApp.Should().BeEquivalentTo(updateJobAppDto);
    }

    [Fact]
    public async Task Update_JobAppNotExist_ReturnsNotFound()
    {
        // Arrange
        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(It.IsAny<Guid>()).Result)
            .Returns((JobApplication?)null);

        // Act
        var result = await JobApplicationEndpoints.Update(
            mock.Object,
            Guid.NewGuid(),
            new UpdateJobApplicationDto
            {
                Company = "Google",
                Role = "People and Culture Manager",
                Status = Status.Closed,
                Link = "https://careers.google.com/12345",
                CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
            }
        );

        // Assert
        result.Should().BeOfType<Results<NoContent, NotFound>>()
            .Which.Result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task Delete_JobAppInDb_DeletesFromDb()
    {
        // Arrange
        var dbJobAppId = Guid.NewGuid();
        var dbJobApp = new JobApplication
        {
            Id = dbJobAppId,
            Company = "Microsoft",
            Role = "Software Engineer",
            Status = Status.Applied,
            Link = "https://jobs.microsoft.com/12345",
            CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
        };

        List<JobApplication> jobs = new() { dbJobApp };

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(It.Is<Guid>(id => id == dbJobAppId)).Result)
            .Returns(dbJobApp);

        mock.Setup(service => service.DeleteAsync(It.Is<JobApplication>(j => j.Id == dbJobAppId)))
            .Callback<JobApplication>(deletedJobApp => jobs.Remove(deletedJobApp))
            .Returns(Task.CompletedTask);

        // Act
        var result = await JobApplicationEndpoints.Delete(mock.Object, dbJobAppId);

        // Assert
        result.Should().BeOfType<Results<NoContent, NotFound>>()
            .Which.Result.Should().BeOfType<NoContent>();

        jobs.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_JobAppNotExist_ReturnsNotFound()
    {
        // Arrange
        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAsync(It.IsAny<Guid>()).Result)
            .Returns((JobApplication?)null);

        // Act
        var result = await JobApplicationEndpoints.Delete(mock.Object, Guid.NewGuid());

        // Assert
        result.Should().BeOfType<Results<NoContent, NotFound>>()
            .Which.Result.Should().BeOfType<NotFound>();
    }
}
