using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GtMotive.Fleet.Infrastructure.Persistence
{
    public sealed class FleetDbContextFactory : IDesignTimeDbContextFactory<FleetDbContext>
    {
        private const string LocalDevelopmentConnectionString =
            "Host=localhost;Port=5432;Database=fleet;Username=fleet;Password=fleet";

        public FleetDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__FleetDb")
                ?? LocalDevelopmentConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<FleetDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new FleetDbContext(optionsBuilder.Options);
        }
    }
}
