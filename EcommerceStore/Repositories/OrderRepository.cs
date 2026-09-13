using ECommerceStore.Data;
using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceContext _eCommerceContext;
        public OrderRepository(ECommerceContext eCommerceContext)  
        {
            _eCommerceContext = eCommerceContext;
        }
        public async Task<Order> CreateOrderByUserIdAsync(Order newOrder)
        {
            //var findexistingOrder = await _eCommerceContext.Orders
            //    .FirstOrDefaultAsync(o => o.UserId == newOrder.UserId && o.Status == OrderStatus.Pending);
            //if (findexistingOrder != null)
            //{
            //    // Handle existing pending order logic here
            //}
            await _eCommerceContext.Orders.AddAsync(newOrder);
            await _eCommerceContext.SaveChangesAsync();
            return newOrder;
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            return await _eCommerceContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _eCommerceContext.Orders
                .Include(o => o.OrderItems)
                .Include(s => s.ShippingAddressDetails)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task DeleteItemsFromCartAsync(List<int> productIds)
        {
            _eCommerceContext.CartItems.RemoveRange(_eCommerceContext.CartItems.Where(ci => productIds.Contains(ci.ProductId)));
            await _eCommerceContext.SaveChangesAsync();
            
        }
    }
}
