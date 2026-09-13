using ECommerceStore.Configurations;
using ECommerceStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECommerceStore.Data
{
    public class ECommerceContext : IdentityDbContext<ApplicationUser>
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            // Or automatically scan the assembly for all configuration classes:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
