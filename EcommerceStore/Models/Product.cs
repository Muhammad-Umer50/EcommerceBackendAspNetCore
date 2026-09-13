namespace ECommerceStore.Models
{
    public class Product
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
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
