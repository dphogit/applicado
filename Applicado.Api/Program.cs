using Applicado.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("Applicado"));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var jobAppsRoutes = app.MapGroup("/job-applications");

jobAppsRoutes.MapGet("/", async (DataContext context) => await context.JobApplications.ToArrayAsync());

app.Run();
