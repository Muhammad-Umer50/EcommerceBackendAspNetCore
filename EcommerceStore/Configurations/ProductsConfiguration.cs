using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
             
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.IsAvailable)
                .IsRequired();

            builder.Property(p => p.StockQuantity)
                .IsRequired();
            builder.Property(p => p.rating)
                .IsRequired()
                .HasColumnType("decimal(3,2)");
            // One Product -> Many ProductImages
            builder.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);   // delete images when product is deleted

        }
    }
}
