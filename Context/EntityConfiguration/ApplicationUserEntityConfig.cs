using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.EntityConfiguration
{
    internal class ApplicationUserEntityConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Names)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Surnames)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Address)
                .HasMaxLength(200);

            builder.Property(x => x.City)
                .HasMaxLength(60);

            builder.Property(x => x.Country)
                .HasMaxLength(60);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500);

            builder.Ignore(x => x.Role);
        }
    }
}