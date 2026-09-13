using ECommerceStore.Dtos.CartDtos;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet("GetCartItems")]
        public async Task<IActionResult> GetCartItemsByCartId(int cartId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var cart = await _cartService.GetCartItemsAsync(cartId, userId);
            if(cart == null)
            {
                return NotFound("Cart not found for the user.");
            }
            return Ok(cart);
        }
        [Authorize]
        [HttpPost("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart(int cartId, CreateCartItemsDto item)
        {
            // Call the service to add an item to the cart for the user
            await _cartService.AddItemToCartAsync(cartId, item);
            return Ok(new { Message = "Item added to cart successfully." });
        }
        [Authorize]
        [HttpDelete("RemoveItemFromCart")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int productId)
        {
            // Call the service to remove an item from the cart for the user
            await _cartService.RemoveItemFromCartAsync(cartId, productId);
            return Ok(new { Message = "Item removed from cart successfully." });
        }
    }
}
