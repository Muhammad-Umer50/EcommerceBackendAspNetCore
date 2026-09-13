using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.RequestParameters;

namespace ECommerceStore.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<(List<Product> Items, int TotalCount)> GetProductsAsync(ProductQueryParameters query);
        Task<Product> GetProductByIdAsync(int id);
        Task AddAsync(Product product);          // <-- add
        Task SaveChangesAsync();
        Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Product>> GetProductsforSuggestion(string searchTerm);


    }
}
