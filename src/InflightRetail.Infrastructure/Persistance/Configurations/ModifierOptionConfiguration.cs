using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class ModifierOptionConfiguration : IEntityTypeConfiguration<ModifierOption>
{
    public void Configure(EntityTypeBuilder<ModifierOption> builder)
    {
        builder.ToTable("modifier_options");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        // PostgreSQL numeric tipi için precision ve scale ayarı
        builder.Property(x => x.PriceDelta)
            .HasPrecision(10, 2)
            .IsRequired()
            .HasDefaultValue(0.00m);

        builder.Property(x => x.SkuCode)
            .HasMaxLength(50);

        builder.Property(x => x.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}