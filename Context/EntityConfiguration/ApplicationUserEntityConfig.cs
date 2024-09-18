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

            // Configuración de las propiedades heredadas de IdentityUser
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(500);

            // Relación con otras entidades si fuera necesario
            // Por ejemplo: Si un ApplicationUser puede tener propiedades o cualquier otra relación
            // Si no tienes relaciones, puedes omitir esta sección.

            //builder.HasMany(x => x.Properties) // Relación hipotética
            //       .WithOne(x => x.Owner)
            //       .HasForeignKey(x => x.OwnerId)
            //       .OnDelete(DeleteBehavior.Cascade);

            // Ignorar la propiedad Role ya que no debe ser mapeada a la base de datos
            builder.Ignore(x => x.Role);
        }
    }
}