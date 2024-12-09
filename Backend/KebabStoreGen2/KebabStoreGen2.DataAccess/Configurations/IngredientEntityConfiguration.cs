using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KebabStoreGen2.DataAccess.Configurations;

public class IngredientEntityConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.ToTable("Ingredients");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(i => i.IngredientName)
            .IsRequired()
            .HasMaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH);

        builder.Property(i => i.CaloriesPer100g)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.ProteinPer100g)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.FatPer100g)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.CarbsPer100g)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.SugarPer100g)
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.ContainsLactose)
            .HasColumnType("bit");
    }
}