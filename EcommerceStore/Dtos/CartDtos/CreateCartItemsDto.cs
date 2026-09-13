namespace ECommerceStore.Dtos.CartDtos
{
    public class CreateCartItemsDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
