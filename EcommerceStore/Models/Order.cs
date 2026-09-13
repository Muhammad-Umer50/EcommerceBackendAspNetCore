namespace ECommerceStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Customer
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        // Order information
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
        public ICollection<OrderItems> OrderItems { get; set; }
            = new List<OrderItems>();

        public ShippingAddressDetails ShippingAddressDetails { get; set; } 

    }
}
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded
}