using AutoMapper;
using ECommerceStore.AutoMaprConfiguration;
using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ECommerceStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<GetCategoriesNamesDto>> GetCategoriesNames()
        {
            var result = await _categoryRepository.GetAllCategoriesNamesAsync();
            var categories = _mapper.Map<List<GetCategoriesNamesDto>>(result);
            return categories;
        }

        public async Task<GetCategoriesDto> GetCategoryByIdAsync(int categoryId)
        {
            var result = await _categoryRepository.GetCategoryById(categoryId);

            if (result == null)
            {
                throw new Exception($"Category with ID {categoryId} not found.");
            }

            var category = _mapper.Map<GetCategoriesDto>(result);

            return category;
        }
    }
}
