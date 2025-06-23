using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Image)
            .HasMaxLength(1000);

        builder.OwnsOne(p => p.Rating, rating =>
        {
            rating.Property(r => r.Rate)
                .HasColumnName("Rating_Rate")
                .HasColumnType("decimal(3,2)");

            rating.Property(r => r.Count)
                .HasColumnName("Rating_Count");
        });

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100);

        // TODO: Check if the IsDeleted property is needed (Create an interface for soft delete)
        //builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
