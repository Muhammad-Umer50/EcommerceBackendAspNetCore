using ECommerceStore.Models;

namespace ECommerceStore.Repositories.Interfaces
{
  public  interface IProductImageRepository
    {
        Task<ProductImage> AddAsync(ProductImage image);
        Task<List<ProductImage>> GetByProductIdAsync(int productId);
        Task<ProductImage> GetByIdAsync(int imageId);
        Task DeleteAsync(ProductImage image);
        Task<bool> ProductExistsAsync(int productId);
        Task SaveChangesAsync();
    }
}
