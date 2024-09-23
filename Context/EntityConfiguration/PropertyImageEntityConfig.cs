using Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.EntityConfiguration
{
    internal class PropertyImageEntityConfig : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.HasKey(o => o.IdPropertyImage);

            builder.Property(x => x.IdPropertyImage)
                .IsRequired();

            builder.Property(x => x.File)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Img)
                .HasColumnType("varbinary(max)")
                .IsRequired(false); // Almacenar la foto como bytes

            builder.Property(x => x.Enabled)
                .IsRequired();

            // Relations
            builder.HasOne(x => x.Property)
                   .WithMany(x => x.PropertyImages)
                   .HasForeignKey(x => x.IdProperty)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}