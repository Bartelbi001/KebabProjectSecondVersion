using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KebabStoreGen2.DataAccess.Configurations;

public class KebabEntityConfiguration : IEntityTypeConfiguration<KebabEntity>
{
    public void Configure(EntityTypeBuilder<KebabEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(k => k.KebabName)
            .IsRequired()
            .HasMaxLength(KebabConstants.MAX_KEBABNAME_LENGTH);

        builder.Property(k => k.KebabDescription)
            .IsRequired()
            .HasMaxLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH);

        builder.Property(k => k.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(k => k.Stuffing)
            .IsRequired();

        builder.Property(k => k.Wrap)
            .IsRequired();

        builder.Property(k => k.Calories)
            .IsRequired();

        builder.HasMany(k => k.Ingredients)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}