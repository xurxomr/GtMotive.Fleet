using System;
using GtMotive.Fleet.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GtMotive.Fleet.Infrastructure.Persistence.Configurations
{
    public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Vehicles");
            builder.HasKey(vehicle => vehicle.Id);

            builder.Property(vehicle => vehicle.LicensePlate)
                .HasConversion(licensePlate => licensePlate.Value, value => LicensePlate.Create(value))
                .HasMaxLength(16)
                .IsRequired();

            builder.HasIndex(vehicle => vehicle.LicensePlate).IsUnique();

            builder.Property(vehicle => vehicle.ManufacturingDate).IsRequired();

            builder.Property(vehicle => vehicle.Status)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();
        }
    }
}
