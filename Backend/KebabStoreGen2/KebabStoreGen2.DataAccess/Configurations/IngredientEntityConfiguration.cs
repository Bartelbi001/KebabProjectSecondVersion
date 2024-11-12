﻿using KebabStoreGen2.Core.Constants;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KebabStoreGen2.DataAccess.Configurations;

public class IngredientEntityConfiguration : IEntityTypeConfiguration<IngredientEntity>
{
    public void Configure(EntityTypeBuilder<IngredientEntity> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.IngredientName)
            .IsRequired()
            .HasMaxLength(IngredientConstants.MAX_INGREDIENTNAME_LENGTH);

        builder.Property(i => i.WeightInGrams)
            .IsRequired();

        builder.Property(i => i.Calories)
            .IsRequired();

        builder.Property(i => i.Protein)
            .IsRequired();

        builder.Property(i => i.Fat)
            .IsRequired();

        builder.Property(i => i.Carbs)
            .IsRequired();
    }
}