using System.ComponentModel.DataAnnotations;
using Applicado.Api.Models;

namespace Applicado.Api.Features.JobApplications;

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

    public DateTimeOffset CloseAtDateTime { get; init; }

    public string? Notes { get; init; }

    public DateTimeOffset CreatedAtDateTime { get; init; }

    public DateTimeOffset UpdatedAtDateTime { get; init; }
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

    public DateTimeOffset CloseAtDateTime { get; init; }

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

    public DateTimeOffset CloseAtDateTime { get; init; }

    public string? Notes { get; init; }
}
