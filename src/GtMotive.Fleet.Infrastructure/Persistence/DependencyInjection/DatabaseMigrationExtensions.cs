using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Fleet.Infrastructure.Persistence.DependencyInjection
{
    public static class DatabaseMigrationExtensions
    {
        public static async Task MigrateFleetDatabaseAsync(this IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
