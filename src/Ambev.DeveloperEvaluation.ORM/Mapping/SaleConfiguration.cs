using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.SaleNumber)
            .IsUnique();

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.CustomerId)
            .IsRequired();

        builder.Property(s => s.CartId)
            .IsRequired();

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.CancelationReason)
            .HasMaxLength(500);

        builder.HasOne(s => s.Cart)
            .WithOne(c => c.Sale)
            .HasForeignKey<Sale>(s => s.CartId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.SaleItems)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
