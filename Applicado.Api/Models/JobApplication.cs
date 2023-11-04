using System.ComponentModel.DataAnnotations;

namespace Applicado.Api.Models;

public class JobApplication
{
    public Guid Id { get; set; }

    [Required]
    public required string Company { get; set; }

    [Required]
    public required string Role { get; set; }

    [Required]
    public required Status Status { get; set; }

    public string? Link { get; set; }

    public DateTimeOffset CloseAtDateTime { get; set; }

    public string? Notes { get; set; }
}
