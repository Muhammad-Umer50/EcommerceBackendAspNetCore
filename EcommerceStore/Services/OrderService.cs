using AutoMapper;
using ECommerceStore.Dtos.OrderDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.Services.Interfaces;

namespace ECommerceStore.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task CreateOrderByUserIdAsync(string userId, CreateOrderDto createOrderDto)
        {

            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            if (createOrderDto == null)
                throw new ArgumentNullException(nameof(createOrderDto));


            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingMethod = createOrderDto.ShippingMethod,
                PaymentMethod = createOrderDto.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                ShippingCost = 0, // Calculate shipping cost based on your logic
                Subtotal = 0,
                Tax = 0,
                TotalAmount = 0,
                ShippingAddressDetails = new ShippingAddressDetails
                {
                    Address = createOrderDto.ShippingAddressDetails.Address,
                    City = createOrderDto.ShippingAddressDetails.City,
                    Country = createOrderDto.ShippingAddressDetails.Country
                }
            };

            switch (createOrderDto.ShippingMethod)
            {
                case "Standard":
                    order.ShippingCost = 5.00m;
                    break;
                case "Express":
                    order.ShippingCost = 15.00m;
                    break;
                case "NextDay":
                    order.ShippingCost = 30.00m;
                    break;
                default:
                    order.ShippingCost = 0.00m;
                    break;
            }

            var productIds = createOrderDto.Items
              .Select(x => x.ProductId)
              .ToList();
            var products = await this._orderRepository.GetProductsByIdsAsync(productIds);

            foreach (var product in products)
            {
                var orderItem = new OrderItems
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = createOrderDto.Items.First(x => x.ProductId == product.Id).Quantity,
                    TotalPrice = product.Price * createOrderDto.Items.First(x => x.ProductId == product.Id).Quantity
                };
                order.OrderItems.Add(orderItem);
                // Reduce stock
                product.StockQuantity -= orderItem.Quantity;
            }
            order.Subtotal = order.OrderItems.Sum(x => x.TotalPrice);
            order.Tax = order.Subtotal * 0.5m;
            order.TotalAmount =
                order.Subtotal + order.ShippingCost;

            await this._orderRepository.CreateOrderByUserIdAsync(order);

            await this._orderRepository.DeleteItemsFromCartAsync(productIds);
        }
        public async Task<List<GetOrderDto>?> GetOrdersByUserIdAsync(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            var order = await this._orderRepository.GetOrdersByUserIdAsync(userId);
            if (order == null)
                return null;

            var getOrderDto = _mapper.Map<List<GetOrderDto>>(order);
            return getOrderDto;
        }
    }
}
