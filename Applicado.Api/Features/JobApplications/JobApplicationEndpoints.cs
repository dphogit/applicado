using Applicado.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Applicado.Api.Features.JobApplications;

public static class JobApplicationEndpoints
{
    public static RouteGroupBuilder MapJobApplicationEndpoints(this RouteGroupBuilder jobAppsRouteGroup)
    {
        jobAppsRouteGroup.MapGet("/", JobApplicationEndpoints.GetAll);
        jobAppsRouteGroup.MapGet("/{id}", JobApplicationEndpoints.Get).WithName("GetById");
        jobAppsRouteGroup.MapPost("/", JobApplicationEndpoints.Create);
        jobAppsRouteGroup.MapPut("/{id}", JobApplicationEndpoints.Update);
        jobAppsRouteGroup.MapDelete("/{id}", JobApplicationEndpoints.Delete);
        return jobAppsRouteGroup;
    }

    public static async Task<Ok<JobApplicationDto[]>> GetAll(IJobApplicationsService service)
    {
        var jobApplications = await service.GetAllAsync();
        var jobAppDtos = jobApplications.Select(j => j.AsDto()).ToArray();
        return TypedResults.Ok(jobAppDtos);
    }

    public static async Task<Results<Ok<JobApplicationDto>, NotFound>> Get(IJobApplicationsService service, Guid id)
    {
        var jobApp = await service.GetAsync(id);
        if (jobApp is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(jobApp.AsDto());
    }

    public static async Task<CreatedAtRoute<JobApplicationDto>> Create(
        IJobApplicationsService service,
        CreateJobApplicationDto jobAppDto)
    {
        var newJobApp = new JobApplication
        {
            Id = Guid.NewGuid(),
            Company = jobAppDto.Company,
            Role = jobAppDto.Role,
            Status = jobAppDto.Status,
            Link = jobAppDto.Link,
            CloseAtDateTime = jobAppDto.CloseAtDateTime,
            Notes = jobAppDto.Notes
        };

        await service.CreateAsync(newJobApp);

        return TypedResults.CreatedAtRoute(
            routeName: "GetById",
            routeValues: new { id = newJobApp.Id.ToString() },
            value: newJobApp.AsDto()
        );
    }

    public static async Task<Results<NoContent, NotFound>> Update(
        IJobApplicationsService service,
        Guid id,
        UpdateJobApplicationDto jobAppDto)
    {
        var existingJobApp = await service.GetAsync(id);

        if (existingJobApp is null)
            return TypedResults.NotFound();

        existingJobApp.Company = jobAppDto.Company;
        existingJobApp.Role = jobAppDto.Role;
        existingJobApp.Status = jobAppDto.Status;
        existingJobApp.Link = jobAppDto.Link;
        existingJobApp.CloseAtDateTime = jobAppDto.CloseAtDateTime;
        existingJobApp.Notes = jobAppDto.Notes;

        await service.UpdateAsync(existingJobApp);
        return TypedResults.NoContent();
    }


    public static async Task<NoContent> Delete(IJobApplicationsService service, Guid id)
    {
        var jobApp = await service.GetAsync(id);

        if (jobApp is null)
            return TypedResults.NoContent();

        await service.DeleteAsync(jobApp);
        return TypedResults.NoContent();
    }
}
