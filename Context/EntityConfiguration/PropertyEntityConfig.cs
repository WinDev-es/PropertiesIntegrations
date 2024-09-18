using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.EntityConfiguration
{
    internal class PropertyEntityConfig : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(o => o.IdProperty);

            builder.Property(x => x.IdProperty)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.State)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.ImageURL)
                .IsRequired(false)
                .HasMaxLength(500);

            // Relations
            builder.HasMany(x => x.PropertyImages)
                   .WithOne(x => x.Property)
                   .HasForeignKey(x => x.IdProperty)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.PropertyTraces)
                   .WithOne(x => x.Property)
                   .HasForeignKey(x => x.IdProperty)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
