using System;
using GtMotive.Fleet.Domain.Rentals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GtMotive.Fleet.Infrastructure.Persistence.Configurations
{
    public sealed class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Rentals");
            builder.HasKey(rental => rental.Id);

            builder.Property(rental => rental.VehicleId).IsRequired();

            builder.Property(rental => rental.RenterId)
                .HasConversion(renterId => renterId.Value, value => RenterId.Create(value))
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(rental => rental.StartedOn).IsRequired();

            builder.Property(rental => rental.Status)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();

            builder.HasIndex(rental => rental.RenterId)
                .IsUnique()
                .HasFilter("\"Status\" = 'Active'");
        }
    }
}
