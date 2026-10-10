using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("flights");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FlightNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Origin)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.Destination)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.AircraftType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasMany(x => x.MenuItems)
            .WithOne(x => x.Flight)
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.CurrencyRates)
            .WithOne(x => x.Flight)
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}