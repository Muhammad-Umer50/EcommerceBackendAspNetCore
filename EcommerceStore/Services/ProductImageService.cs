using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerceStore.Services
{
    public class ProductImageService: IproductImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IProductImageRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductImageService(IWebHostEnvironment webHostEnvironment, IProductImageRepository repository,IHttpContextAccessor httpContextAccessor)
        {
            _env = webHostEnvironment;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public async Task<ProductImageDto> UploadImageAsync(int productId, IFormFile file, bool isMain = false)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Allowed: jpg, jpeg, png, webp.");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File too large. Max size is 5MB.");

            var productExists = await _repository.ProductExistsAsync(productId);
            if (!productExists)
                throw new KeyNotFoundException($"Product {productId} not found.");

            var productFolder = Path.Combine(_env.WebRootPath, "images", "products", productId.ToString());
            Directory.CreateDirectory(productFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(productFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new ProductImage
            {
                ProductId = productId,
                ImageUrl = $"/images/products/{productId}/{fileName}",
                IsMain = isMain
            };

            await _repository.AddAsync(image);
            await _repository.SaveChangesAsync();

            return MapToDto(image);
        }

        public async Task<List<ProductImageDto>> GetImagesAsync(int productId)
        {
            var images = await _repository.GetByProductIdAsync(productId);
            return images.Select(MapToDto).ToList();
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var image = await _repository.GetByIdAsync(imageId);
            if (image == null)
                throw new KeyNotFoundException($"Image {imageId} not found.");

            var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
                File.Delete(filePath);

            await _repository.DeleteAsync(image);
            await _repository.SaveChangesAsync();
        }

        private ProductImageDto MapToDto(ProductImage image)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return new ProductImageDto
            {
                Id = image.Id,
                Url = $"{baseUrl}{image.ImageUrl}",
                IsMain = image.IsMain
            };
        }


    }
}
