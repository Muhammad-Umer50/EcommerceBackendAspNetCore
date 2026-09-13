using ECommerceStore.Data;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECommerceStore.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ECommerceContext _context;

        public ProductImageRepository(ECommerceContext context)
        {
            _context = context;
        }
        public async Task<ProductImage> AddAsync(ProductImage image)
        {
            await _context.ProductImages.AddAsync(image);
            return image;
        }

        public async Task<List<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductImage> GetByIdAsync(int imageId)
        {
            return await _context.ProductImages.FindAsync(imageId);
        }

        public async Task DeleteAsync(ProductImage image)
        {
            _context.ProductImages.Remove(image);
            await Task.CompletedTask;
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
