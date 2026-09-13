using ECommerceStore.Data;
using ECommerceStore.Dtos.CartDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceContext _context;

        public CartRepository(ECommerceContext context)
        {
            _context = context;
        }
        public async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart { UserId = userId };
            this._context.Carts.Add(cart);
            await this._context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            var cart = await this._context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null) 
                throw new Exception($"Cart not found for user with ID {userId}");
            return cart;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId, string userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Images)
                .Where(ci => ci.CartId == cartId &&
                ci.Cart.UserId == userId)
                .ToListAsync();

            //if (cartItems.Count == 0)
            //    throw new Exception($"Cart items not found for cart with ID {cartId}");

            return cartItems;
        }
        public async Task<CartItem> AddItemToCartAsync(int cartId, CartItem item)
        {
            var cart = await this._context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new Exception($"Cart not found for cart with ID {cartId}");
            bool itemExists = await this._context.CartItems.AnyAsync(ci => ci.CartId == cartId && ci.ProductId == item.ProductId);
            if (itemExists)
                throw new Exception($"Item already exist");
            item.CartId = cart.Id;
            await this._context.CartItems.AddAsync(item);
            await this._context.SaveChangesAsync();
            return item;
        }
        public async Task<string> RemoveItemFromCartAsync(int cartId, int productId)
        {
            var cartItem = await this._context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
            if (cartItem == null)
                throw new Exception($"Cart item not found for cart ID {cartId} and product ID {productId}");
            this._context.CartItems.Remove(cartItem);
            await this._context.SaveChangesAsync();
            return $"Item with product ID {productId} removed from cart with ID {cartId}";
        }


    }
}
