using ECommerceStore.Data;
using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Repositories.Interfaces
{
    public interface ICategoryRepository 
    {
        Task<Category> GetCategoryById(int categoryId);
        Task<List<Category>> GetAllCategoriesNamesAsync();
       
    }
}
