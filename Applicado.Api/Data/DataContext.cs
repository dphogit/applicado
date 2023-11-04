using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Applicado.Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<JobApplication> JobApplications { get; set; }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.Entity is TimestampedModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

        DateTimeOffset now = DateTimeOffset.UtcNow;

        foreach (var entityEntry in entities)
        {
            var baseEntity = (TimestampedModel)entityEntry.Entity;
            baseEntity.UpdatedAtDateTime = now;

            if (entityEntry.State == EntityState.Added)
            {
                baseEntity.CreatedAtDateTime = now;
            }
        }
    }
}
