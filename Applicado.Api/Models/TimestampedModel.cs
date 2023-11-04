namespace Applicado.Api.Models;
public abstract class TimestampedModel
{
    public DateTimeOffset CreatedAtDateTime { get; set; }
    public DateTimeOffset UpdatedAtDateTime { get; set; }
}
