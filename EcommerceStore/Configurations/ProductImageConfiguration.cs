
using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(i => i.IsMain)
             .HasDefaultValue(false);

            builder.HasOne(i => i.Product)
             .WithMany(p => p.Images)
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
