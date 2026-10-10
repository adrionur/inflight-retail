using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Sku)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.BasePrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("EUR");

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}