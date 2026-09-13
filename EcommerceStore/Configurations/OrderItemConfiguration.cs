using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class OrderItemConfiguration:IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            // Primary Key
            builder.HasKey(oi => oi.Id);

            // Order relationship
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product relationship
            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product snapshot
            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            // Purchase information
            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
}
