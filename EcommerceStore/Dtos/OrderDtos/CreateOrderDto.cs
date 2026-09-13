namespace ECommerceStore.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = new();

        public string ShippingMethod { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public ShippingAddressDetailsDto ShippingAddressDetails { get; set; } = new();

        //public BillingAddressDto BillingAddress { get; set; } = null!;
    }
}
