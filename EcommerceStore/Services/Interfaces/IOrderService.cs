using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderByUserIdAsync(string userId, CreateOrderDto createOrderDto);
        Task<List<GetOrderDto>?> GetOrdersByUserIdAsync(string userId);
    }
}
