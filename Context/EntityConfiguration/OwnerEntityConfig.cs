using DataTransferObjets.Configuration;
using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.EntityConfiguration
{
    internal class OwnerEntityConfig : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(o => o.IdOwner);

            builder.Property(x => x.IdOwner)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.Birthday)
                .IsRequired();

            builder.Property(x => x.Photo)
                .HasColumnType("varbinary(max)")
                .IsRequired(false); // Almacenar la foto como bytes

            // Relations

            builder.HasMany(x => x.Properties)
                   .WithOne(x => x.Owner)
                   .HasForeignKey(x => x.IdOwner)
                   .OnDelete(DeleteBehavior.NoAction);

            //// Data Default for Test
            builder.HasData(
            new Owner
            {
                IdOwner = Guid.Parse(StaticDefination.IdDefaultOwner),
                Name = "Mr. Afghanistan",
                Address = "ST AFG False",
                Birthday = DateTime.Now
            });
        }
    }
}