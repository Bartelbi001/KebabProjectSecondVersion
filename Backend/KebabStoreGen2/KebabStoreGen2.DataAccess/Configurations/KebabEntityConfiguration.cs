using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KebabStoreGen2.DataAccess.Configurations;

public class KebabEntityConfiguration : IEntityTypeConfiguration<KebabEntity>
{
    public void Configure(EntityTypeBuilder<KebabEntity> builder)
    {
        builder.ToTable("Kebabs");

        builder.HasKey(x => x.Id);

        builder.Property(i => i.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(k => k.KebabName)
            .IsRequired()
            .HasMaxLength(KebabConstants.MAX_KEBABNAME_LENGTH);

        builder.Property(k => k.KebabDescription)
            .IsRequired()
            .HasMaxLength(KebabConstants.MAX_KEBABDESCRIPTION_LENGTH);

        builder.Property(k => k.KebabPrice)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(k => k.Stuffing)
            .IsRequired();

        builder.Property(k => k.Wrap)
            .IsRequired();

        builder.Property(k => k.IsAvailable)
            .IsRequired();

        builder.HasMany(k => k.KebabIngredients)
            .WithOne(ki => ki.Kebab)
            .HasForeignKey(ki => ki.KebabId)
            .IsRequired();

        builder.Property(k => k.TotalWeight)
            .IsRequired();

        builder.Property(k => k.TotalCalories)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(k => k.TotalCarbs)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(k => k.TotalFat)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(k => k.TotalProtein)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");
    }
}