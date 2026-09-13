using ECommerceStore.Dtos.CartDtos;

namespace ECommerceStore.Dtos.AuthDtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Email { get; set; }
        public int CartId { get; set; }
    }
}
