using ECommerceStore.Dtos.CartDtos;

namespace ECommerceStore.Services.Interfaces
{
    public interface ICartService
    {
        Task<CreateCartDto> CreateCartAsync(string userId);
        Task<GetCartDto> GetCartByUserIdAsync(string userId);
        Task<List<GetCartItemsDto>> GetCartItemsAsync(int cartId, string userId);
        Task<string> AddItemToCartAsync(int cartId, CreateCartItemsDto item);
        Task<string> RemoveItemFromCartAsync(int cartId, int productId);
    }
}
