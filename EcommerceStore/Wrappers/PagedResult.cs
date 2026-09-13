using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;

namespace ECommerceStore.Wrappers
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public List<ProductImageDto> Images { get; internal set; }
        public List<string> CategoryNames { get; set; }
    }
}
