using KebabStoreGen2.Core.Models;
using KebabStoreGen2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KebabStoreGen2.DataAccess.Configurations;

public class KebabConfigurations : IEntityTypeConfiguration<KebabEntity>
{
    public void Configure(EntityTypeBuilder<KebabEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(k => k.KebabName)
            .HasMaxLength(Kebab.MAX_KEBABNAME_LENGTH)
            .IsRequired();

        builder.Property(k => k.KebabDescription)
            .HasMaxLength(Kebab.MAX_KEBABDESCRIPTION_LENGTH)
            .IsRequired();

        builder.Property(k => k.Price)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");
    }
}
