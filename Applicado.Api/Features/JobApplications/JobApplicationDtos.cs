using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Applicado.Api.Models;

namespace Api.Applicado.Features.JobApplications;

public record JobApplicationDto
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    public required string Company { get; init; }

    [Required]
    public required string Role { get; init; }

    [Required]
    public Status Status { get; init; }

    public string? Link { get; init; }

    public DateTimeOffset CloseAt { get; init; }

    public string? Notes { get; init; }
}

public record CreateJobApplicationDto
{
    [Required]
    public required string Company { get; init; }

    [Required]
    public required string Role { get; init; }

    [Required]
    public Status Status { get; init; }

    public string? Link { get; init; }

    public DateTimeOffset CloseAt { get; init; }

    public string? Notes { get; init; }
}

public record UpdateJobApplicationDto
{
    [Required]
    public required string Company { get; init; }

    [Required]
    public required string Role { get; init; }

    [Required]
    public Status Status { get; init; }

    public string? Link { get; init; }

    public DateTimeOffset CloseAt { get; init; }

    public string? Notes { get; init; }
}
