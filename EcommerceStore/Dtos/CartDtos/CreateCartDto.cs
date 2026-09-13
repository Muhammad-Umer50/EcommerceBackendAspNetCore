using ECommerceStore.Models;

namespace ECommerceStore.Dtos.CartDtos
{
    public class CreateCartDto
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

    }
}
