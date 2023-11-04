using Api.Applicado.Features.JobApplications;
using Applicado.Api.Models;

namespace Applicado.Api;

public static class Extensions
{
    public static JobApplicationDto AsDto(this JobApplication jobApplication)
    {
        return new JobApplicationDto
        {
            Id = jobApplication.Id,
            Company = jobApplication.Company,
            Role = jobApplication.Role,
            Status = jobApplication.Status,
            Link = jobApplication.Link,
            CloseAt = jobApplication.CloseAt,
            Notes = jobApplication.Notes
        };
    }
}
