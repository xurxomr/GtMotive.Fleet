using System;
using System.Threading.Tasks;
using GtMotive.Fleet.Domain.Interfaces;
using GtMotive.Fleet.Domain.Rentals;
using GtMotive.Fleet.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace GtMotive.Fleet.Infrastructure.Persistence
{
    public sealed class FleetDbContext(DbContextOptions<FleetDbContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public DbSet<Rental> Rentals => Set<Rental>();

        public Task<int> Save() => SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FleetDbContext).Assembly);
        }
    }
}
