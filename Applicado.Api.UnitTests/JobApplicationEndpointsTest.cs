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
    public async void GetAllAsync_JobAppsInDb_ReturnsJobApps()
    {
        // Arrange
        Guid JOB_ID1 = Guid.NewGuid();
        Guid JOB_ID2 = Guid.NewGuid();

        var mock = new Mock<IJobApplicationsService>();
        mock.Setup(service => service.GetAllAsync().Result).Returns(
            new List<JobApplication>
            {
                new()
                {
                    Id = JOB_ID1,
                    Company = "Microsoft",
                    Role = "Software Engineer",
                    Status = Status.Applied,
                    Link = "https://jobs.microsoft.com/12345",
                    CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
                },
                new()
                {
                    Id = JOB_ID2,
                    Company = "Google",
                    Role = "People and Culture Manager",
                    Status = Status.Closed,
                    Link = "https://careers.google.com/12345",
                    CloseAtDateTime = 31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero),
                }
            }
        );

        // Act
        var jobApps = await JobApplicationEndpoints.GetAll(mock.Object);

        // Assert
        jobApps.Should()
            .BeOfType<Ok<JobApplicationDto[]>>()
            .Which.Value.Should()
                .HaveCount(2)
                .And.SatisfyRespectively(
                    jobApp1 =>
                    {
                        jobApp1.Id.Should().Be(JOB_ID1);
                        jobApp1.Company.Should().Be("Microsoft");
                        jobApp1.Role.Should().Be("Software Engineer");
                        jobApp1.Status.Should().Be(Status.Applied);
                        jobApp1.Link.Should().Be("https://jobs.microsoft.com/12345");
                        jobApp1.CloseAtDateTime.Should().Be(31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero));
                    },
                    jobApp2 =>
                    {
                        jobApp2.Id.Should().Be(JOB_ID2);
                        jobApp2.Company.Should().Be("Google");
                        jobApp2.Role.Should().Be("People and Culture Manager");
                        jobApp2.Status.Should().Be(Status.Closed);
                        jobApp2.Link.Should().Be("https://careers.google.com/12345");
                        jobApp2.CloseAtDateTime.Should().Be(31.December(2023).At(23, 59, 59).WithOffset(TimeSpan.Zero));
                    }
                );
    }
}
