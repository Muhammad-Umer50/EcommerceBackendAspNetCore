using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class CartItemsConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Quantity).IsRequired();
            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.CartItems)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
