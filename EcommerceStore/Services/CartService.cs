using AutoMapper;
using ECommerceStore.Dtos.CartDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ECommerceStore.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper) 
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
         
       
        public async Task<CreateCartDto> CreateCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            };

           var createdCart = await _cartRepository.CreateCartAsync(userId);
            var dto = _mapper.Map<CreateCartDto>(createdCart);
            return dto;
        }

        public async Task<GetCartDto> GetCartByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            };

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            var dto = _mapper.Map<GetCartDto>(cart);
            return dto;
        }

        public async Task<List<GetCartItemsDto>> GetCartItemsAsync(int cartId, string userId)
        {
            if (cartId <= 0)
            {
                throw new ArgumentException("CartId must be a positive integer.", nameof(cartId));
            };
            var cartItem = await _cartRepository.GetCartItemsAsync(cartId, userId);
            var dto = _mapper.Map<List<GetCartItemsDto>>(cartItem);
            return dto;
        }
        public async Task<string> AddItemToCartAsync(int cartId, CreateCartItemsDto item)
        {
            if (cartId <= 0)
            {
                throw new ArgumentException("CartId must be a positive integer.", nameof(cartId));
            };
            var cartItem = _mapper.Map<CartItem>(item);
            var addedCartItem = await _cartRepository.AddItemToCartAsync(cartId, cartItem);
           // var dto = _mapper.Map<GetCartItemsDto>(addedCartItem);
            return "Item added to cart successfully.";
        }
        public async Task<string> RemoveItemFromCartAsync(int cartId, int productId)
        {
            if (cartId <= 0)
            {
                throw new ArgumentException("CartId must be a positive integer.", nameof(cartId));
            };
            if (productId <= 0)
            {
                throw new ArgumentException("ProductId must be a positive integer.", nameof(productId));
            };
            var result = await _cartRepository.RemoveItemFromCartAsync(cartId, productId);
            return result;
        }
    }
}
