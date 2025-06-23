using Ambev.DeveloperEvaluation.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItems");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.ProductName)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(si => si.Quantity)
                .IsRequired();

            builder.Property(si => si.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.DiscountPercentage)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(si => si.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(si => si.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(si => si.CancelationReason)
                .HasMaxLength(500);

            builder.HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId);

            builder.HasOne(si => si.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(si => si.ProductId);

            // TODO: Check if the IsDeleted property is needed (Create an interface for soft delete)
            //builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }

}
