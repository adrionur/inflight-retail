using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class ProductModifierGroupConfiguration : IEntityTypeConfiguration<ProductModifierGroup>
{
    public void Configure(EntityTypeBuilder<ProductModifierGroup> builder)
    {
        builder.ToTable("product_modifier_groups");

        // Composite Key
        builder.HasKey(x => new { x.ProductId, x.ModifierGroupId });

        builder.Property(x => x.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductModifierGroups)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ModifierGroup)
            .WithMany(x => x.ProductModifierGroups)
            .HasForeignKey(x => x.ModifierGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}