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
    public required string Status { get; set; }

    public string? Link { get; set; }

    public DateTime? CloseAt { get; set; }

    public string? Notes { get; set; }
}
