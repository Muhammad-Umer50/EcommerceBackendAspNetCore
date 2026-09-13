using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderByUserIdAsync(Order newOrder);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task DeleteItemsFromCartAsync(List<int> productIds);
    }
}
