using ECommerceStore.Data;
using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.RequestParameters;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerceStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly ECommerceContext _eCommerceContext;
        public ProductRepository(ECommerceContext eCommerceContext)
        {
            _eCommerceContext = eCommerceContext;
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            var product = _eCommerceContext.Products.Include(c => c.Categories).Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<(List<Product> Items, int TotalCount)> GetProductsAsync(ProductQueryParameters query)
        {
            var products = _eCommerceContext.Products.Include(c => c.Categories).Include(p => p.Images).AsQueryable();

            Console.Write(products.ToQueryString());
            // Searching
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var term = query.SearchTerm.Trim().ToLower();

                products = products.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term));
            }
            // Total count BEFORE pagination
            var totalCount = await products.CountAsync();

            // Sorting
            products = query.SortBy?.ToLower() switch
            {
                "price" => query.SortDescending
                    ? products.OrderByDescending(p => p.Price)
                    : products.OrderBy(p => p.Price),
                "name" => query.SortDescending
                    ? products.OrderByDescending(p => p.Name)
                    : products.OrderBy(p => p.Name),
                _ => products.OrderBy(p => p.Id) // default stable sort
            };

            // Pagination
            var items = await products
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (items, totalCount);

        }
        
        public async Task AddAsync(Product product)
        {
            await _eCommerceContext.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            await _eCommerceContext.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _eCommerceContext.Categories.ToListAsync();

        }

        public async Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds)
        {
            return await _eCommerceContext.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsforSuggestion(string searchTerm)
        {
            var data = await _eCommerceContext.Products.ToListAsync();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                data = data.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) ||
                p.Description.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            return data;
        }
    }
}
