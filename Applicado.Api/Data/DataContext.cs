using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Applicado.Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<JobApplication> JobApplications { get; set; }
}
