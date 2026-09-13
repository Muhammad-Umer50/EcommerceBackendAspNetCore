using ECommerceStore.Data;
using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ECommerceContext _context;

        public CategoryRepository(ECommerceContext context)
        {
            _context = context;
        }
        public async Task<Category> GetCategoryById(int categoryId)
        {
          var result = await  _context.Categories.Include(c => c.Products).ThenInclude(i => i.Images).FirstOrDefaultAsync(c => c.Id == categoryId);
            if (result == null) {
                throw new Exception($"Category with ID {categoryId} not found.");
            }
            else
            {
                return result;
            }   
        }

        public async Task<List<Category>> GetAllCategoriesNamesAsync()
        {
            var result = await _context.Categories.ToListAsync();
            return result;

        }
    }
}
