using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Dtos.CategoryDtos
{
    public class GetCategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GetProductDto> Products { get; set; } = new();


    }
}
