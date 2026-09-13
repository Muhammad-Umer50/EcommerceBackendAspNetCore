using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<GetCategoriesDto> GetCategoryByIdAsync(int categoryId);
        Task<List<GetCategoriesNamesDto>> GetCategoriesNames();
    }
}
