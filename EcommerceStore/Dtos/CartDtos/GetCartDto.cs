using ECommerceStore.Models;

namespace ECommerceStore.Dtos.CartDtos
{
    public class GetCartDto
    {
        public int Id { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
