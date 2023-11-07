using System.Text.Json.Serialization;
using Applicado.Api.Data;
using Applicado.Api.Features.JobApplications;
using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IJobApplicationsService, JobApplicationService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration["Postgres:ConnectionString"];

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.MapEnum<Status>("status");

    var dataSource = dataSourceBuilder.Build();

    options
        .UseNpgsql(dataSource)
        .UseSnakeCaseNamingConvention();
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();
app.UsePathBase("/api/v1");

app.MapGet("/", () => "Hello From Applicado!");

app.MapGroup("/job-applications")
    .MapJobApplicationEndpoints()
    .WithTags("Job Applications");

app.Run();
