using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class FlightMenuItemConfiguration : IEntityTypeConfiguration<FlightMenuItem>
{
    public void Configure(EntityTypeBuilder<FlightMenuItem> builder)
    {
        builder.ToTable("flight_menu_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OverrideBasePrice)
            .HasPrecision(10, 2);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Bir uçuşta aynı ürün sadece bir kez yer alabilir
        builder.HasIndex(x => new { x.FlightId, x.ProductId })
            .IsUnique();
    }
}