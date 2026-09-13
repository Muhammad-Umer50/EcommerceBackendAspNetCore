using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            await _orderService.CreateOrderByUserIdAsync(userId, createOrderDto);
            return Ok(new { message = "Order created successfully" });
        }
        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var order = await _orderService.GetOrdersByUserIdAsync(userId);
            if (order == null)
                return NotFound("No pending order found for the user.");

            return Ok(order);
        }
    }
}
