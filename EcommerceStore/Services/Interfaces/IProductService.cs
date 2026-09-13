using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.RequestParameters;
using ECommerceStore.Wrappers;

namespace ECommerceStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<GetProductDto>> GetProductsAsync(ProductQueryParameters query);
        Task<GetProductDto> GetProductByIdAsync(int id);
        Task<GetProductDto> CreateProductAsync(CreateProductDto dto);
        Task<List<SearchSuggestionDto>> GetSearchSuggestion(string searchSuggestion);
    }
}
