using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Dtos.CartDtos
{
    public class CartProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = [];
    }
}

