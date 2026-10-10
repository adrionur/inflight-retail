using InflightRetail.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InflightRetail.Infrastructure.Persistence.Configurations;

public class ModifierGroupConfiguration : IEntityTypeConfiguration<ModifierGroup>
{
    public void Configure(EntityTypeBuilder<ModifierGroup> builder)
    {
        builder.ToTable("modifier_groups");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.MinSelection)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.MaxSelection)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // 1-to-Many: ModifierGroup -> ModifierOptions
        builder.HasMany(x => x.Options)
            .WithOne(x => x.ModifierGroup)
            .HasForeignKey(x => x.ModifierGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}