using DataTransferObjets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class PropertyTraceEntityConfig : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.HasKey(o => o.IdPropertyTrace);

        builder.Property(x => x.IdPropertyTrace)
            .IsRequired();

        builder.Property(x => x.DateSale)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(x => x.Tax)
            .HasColumnType("decimal(18, 2)");

        // Relations
        builder.HasOne(x => x.Property)
               .WithMany(x => x.PropertyTraces)
               .HasForeignKey(x => x.IdProperty)
               .OnDelete(DeleteBehavior.NoAction);
    }
}