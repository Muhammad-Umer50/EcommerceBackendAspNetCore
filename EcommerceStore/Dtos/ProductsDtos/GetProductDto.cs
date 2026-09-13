using ECommerceStore.Dtos.CategoryDtos;

namespace ECommerceStore.Dtos.ProductsDtos
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal PriceDiscount { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }
        public int rating { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();

       

    }
}
