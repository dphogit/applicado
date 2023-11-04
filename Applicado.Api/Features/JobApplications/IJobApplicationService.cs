using Applicado.Api.Models;

namespace Applicado.Api.Features.JobApplications;

public interface IJobApplicationsService
{
    Task CreateAsync(JobApplication jobApplication);

    Task DeleteAsync(JobApplication jobApplication);

    Task<List<JobApplication>> GetAllAsync();

    Task<JobApplication?> GetAsync(Guid id);

    Task UpdateAsync(JobApplication jobApplication);
}
