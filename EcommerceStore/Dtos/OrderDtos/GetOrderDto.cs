using ECommerceStore.Models;

namespace ECommerceStore.Dtos.OrderDtos
{
    public class GetOrderDto
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Payment
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string PaymentMethod { get; set; } = string.Empty;

        // Shipping 
        public string ShippingMethod { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }

        // Amounts
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }

        // Relationships
        public ICollection<GetOrderItemsDto> OrderItems { get; set; }
            = new List<GetOrderItemsDto>();

        public ShippingAddressDetailsDto? ShippingAddressDetails { get; set; }
    }
}