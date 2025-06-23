using Ambev.DeveloperEvaluation.Domain.Aggregates;
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

        builder.Property(s => s.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BranchId)
            .IsRequired();

        builder.Property(s => s.BranchName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(s => s.CancelationReason)
            .HasMaxLength(500);

        builder.HasMany(s => s.SaleItems)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // TODO: Check if the IsDeleted property is needed (Create an interface for soft delete)
        //builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
