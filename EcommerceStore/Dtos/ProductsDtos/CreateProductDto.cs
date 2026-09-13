namespace ECommerceStore.Dtos.ProductsDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile> Images { get; set; } = new();   // multiple images allowed
        public List<int> CategoryIds { get; set; } = new();

    }
}
