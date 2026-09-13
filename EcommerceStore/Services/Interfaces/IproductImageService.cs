using ECommerceStore.Dtos.ProductsDtos;

namespace ECommerceStore.Services.Interfaces
{
    public interface IproductImageService
    {
        Task<ProductImageDto> UploadImageAsync(int productId, IFormFile file, bool isMain = false);
        Task<List<ProductImageDto>> GetImagesAsync(int productId);
        Task DeleteImageAsync(int imageId);
    }
}
