using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class OrderCofiguration:IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // User relationship
            builder.Property(o => o.UserId)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order date
            builder.Property(o => o.OrderDate)
            //    .IsRequired()
                ;

            // Enums
            builder.Property(o => o.Status)
               // .IsRequired()
                ;

            builder.Property(o => o.PaymentStatus)
              //  .IsRequired()
                ;

            // Payment
            builder.Property(o => o.PaymentMethod)
               // .IsRequired()
                .HasMaxLength(50);

            // Shipping
            builder.Property(o => o.ShippingMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.ShippingCost)
                .HasPrecision(18, 2);

            // Amounts
            builder.Property(o => o.Subtotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.Tax)
                .HasPrecision(18, 2);

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            // Order → OrderItems
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}
