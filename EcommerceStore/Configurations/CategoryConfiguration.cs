using ECommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceStore.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
             builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.HasMany(c => c.Products)
                .WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                "ProductCategory",
                j => j.HasOne<Product>().WithMany().HasForeignKey("ProductsId"),
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                j =>
                {
                    j.HasKey("ProductsId", "CategoryId");
                    j.ToTable("ProductCategories");
                }
                );
        }
    }
}
