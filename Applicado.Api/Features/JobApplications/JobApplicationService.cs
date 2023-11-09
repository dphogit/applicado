using Applicado.Api.Data;
using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Applicado.Api.Features.JobApplications;

class JobApplicationService : IJobApplicationsService
{
    private readonly DataContext dataContext;

    public JobApplicationService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task CreateAsync(JobApplication jobApplication)
    {
        dataContext.JobApplications.Add(jobApplication);
        await dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(JobApplication jobApplication)
    {
        dataContext.JobApplications.Remove(jobApplication);
        await dataContext.SaveChangesAsync();
    }

    public async Task<JobApplication?> GetAsync(Guid id)
    {
        return await dataContext.JobApplications.FindAsync(id);
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        return await dataContext.JobApplications.ToListAsync();
    }

    public async Task UpdateAsync(JobApplication jobApplication)
    {
        dataContext.JobApplications.Update(jobApplication);
        await dataContext.SaveChangesAsync();
    }
}
