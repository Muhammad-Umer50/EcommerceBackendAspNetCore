using ECommerceStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateCartAsync(string userId);
        Task<List<CartItem>> GetCartItemsAsync(int cartId, string userId);
        Task<CartItem> AddItemToCartAsync(int cartId, CartItem item);
        Task<string> RemoveItemFromCartAsync(int cartId, int productId);
    }
}
