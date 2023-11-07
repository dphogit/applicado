using System.Text.Json.Serialization;
using Applicado.Api.Data;
using Applicado.Api.Features.JobApplications;
using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/api/v1");

app.MapGet("/", () => "Hello From Applicado!")
    .WithName("Root")
    .WithTags("Root");

app.MapGroup("/job-applications")
    .MapJobApplicationEndpoints()
    .WithOpenApi()
    .WithTags("Job Applications");

app.Run();
