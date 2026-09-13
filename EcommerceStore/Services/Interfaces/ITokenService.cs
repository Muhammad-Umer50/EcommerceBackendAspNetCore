using ECommerceStore.Models;

namespace ECommerceStore.Services.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiresAt) CreateToken(ApplicationUser user, IList<string> roles);
    }
}
