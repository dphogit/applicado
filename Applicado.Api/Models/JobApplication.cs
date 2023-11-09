using System.ComponentModel.DataAnnotations;

namespace Applicado.Api.Models;

public class JobApplication : TimestampedModel
{
    public Guid Id { get; set; }

    [Required]
    public required string Company { get; set; }

    [Required]
    public required string Role { get; set; }

    public Status? Status { get; set; }

    public string? Link { get; set; }

    public DateTimeOffset? CloseAtDateTime { get; set; }

    public string? Notes { get; set; }
}
