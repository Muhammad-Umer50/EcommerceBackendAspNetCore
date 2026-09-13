namespace ECommerceStore.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageUrl { get; set; }   // relative path or full URL
        public bool IsMain { get; set; }
    }
}
