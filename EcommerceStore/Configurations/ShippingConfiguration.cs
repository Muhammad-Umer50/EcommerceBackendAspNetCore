using ECommerceStore.Migrations;
using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class ShippingConfiguration : IEntityTypeConfiguration<ShippingAddressDetails>
    {
        public void Configure(EntityTypeBuilder<ShippingAddressDetails> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Address)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.City)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.Country)
                   .IsRequired()
                   .HasMaxLength(50);
           builder.HasOne(s => s.Order)
                   .WithOne(o => o.ShippingAddressDetails)
                   .HasForeignKey<ShippingAddressDetails>(s => s.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
