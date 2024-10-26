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

        builder.Property(k => k.Name)
            .HasMaxLength(Kebab.MAX_KEBABNAME_LENGTH)
            .IsRequired();

        builder.Property(k => k.Description)
            .HasMaxLength(Kebab.MAX_KEBABDESCRIPTION_LENGTH)
            .IsRequired();

        builder.Property(k => k.Price)
            .IsRequired();
    }
}
