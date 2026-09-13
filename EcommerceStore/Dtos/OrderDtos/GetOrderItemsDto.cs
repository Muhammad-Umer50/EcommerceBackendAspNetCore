using ECommerceStore.Models;

namespace ECommerceStore.Dtos.OrderDtos
{
    public class GetOrderItemsDto
    {
        public int OrderId { get; set; }
      //  public Order Order { get; set; } = null!;

        // Product
        public int ProductId { get; set; }
      //  public Product Product { get; set; } = null!;

        // Product snapshot
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }

        // Purchase information
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
