using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultitenantDbPoc.Options;

namespace MultitenantDbPoc.Persistence;

public class ApplicationDbContext : DbContext
{

    // public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, IOptionsSnapshot<DbConfiguration> dbConfiguration) : base(dbContextOptions)
    // {
    //     _dbConfiguration = dbConfiguration;
    // }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    // public void SetTenant(string tenantId)
    // {
    //     if (_dbConfiguration?.Value.TenantConnectionStrings.TryGetValue(tenantId, out var connectionString))
    //     {
    //         Database.SetConnectionString(connectionString);
    //     }
    //
    //     Database.SetConnectionString(_dbConfiguration.Value.DefaultConnectionString);
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}