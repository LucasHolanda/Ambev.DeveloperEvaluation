using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.Date)
                .IsRequired();

            builder.HasMany(c => c.CartProducts)
                .WithOne(cp => cp.Cart)
                .HasForeignKey(cp => cp.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // TODO: Check if the IsDeleted property is needed (Create an interface for soft delete)
            //builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }

}
