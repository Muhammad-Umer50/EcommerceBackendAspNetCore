using AutoMapper;
using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.Repositories.Interfaces;
using ECommerceStore.RequestParameters;
using ECommerceStore.Services.Interfaces;
using ECommerceStore.Wrappers;

namespace ECommerceStore.Services
{
    public class ProductService : IProductService
    {
        readonly IProductRepository _productRepository;
        readonly IMapper _mapper;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IWebHostEnvironment _env;
        public ProductService(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        public async Task<GetProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception($"Product with Id {id} not found.");
            var dto = _mapper.Map<GetProductDto>(product);
            return dto;
        }

        public async Task<PagedResult<GetProductDto>> GetProductsAsync(ProductQueryParameters query)
        {
            var (items, totalCount) = await _productRepository.GetProductsAsync(query);

            var dtos = _mapper.Map<List<GetProductDto>>(items);
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            foreach (var product in dtos)
            {
                foreach (var image in product.Images)
                {
                    image.Url = $"{baseUrl}{image.Url}";
                }
            }


            return new PagedResult<GetProductDto>
            {
                Items = dtos,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                Images = dtos.SelectMany(p => p.Images).ToList(),

            };
        }
        public async Task<GetProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var categories = await _productRepository.GetCategoriesByIdsAsync(dto.CategoryIds);

            if (categories.Count != dto.CategoryIds.Count)
                throw new Exception("One or more CategoryIds are invalid.");

            // 1. Create the product entity first (no images yet — need the generated Id for the folder)
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Images = new List<ProductImage>(),
                Categories = categories

            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();   // saves product, generates product.Id

            // 2. Save each uploaded image to disk, attach to product
            if (dto.Images != null && dto.Images.Any())
            {
                var productFolder = Path.Combine(_env.WebRootPath, "images", "products", product.Id.ToString());
                Directory.CreateDirectory(productFolder);

                bool isFirst = true;
                foreach (var file in dto.Images)
                {
                    ValidateImage(file);

                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(productFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    product.Images.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"/images/products/{product.Id}/{fileName}",
                        IsMain = isFirst   // first uploaded image becomes the main image
                    });

                    isFirst = false;
                }

                await _productRepository.SaveChangesAsync();   // saves images
            }

            // 3. Map to DTO for the response, with full image URLs
            var resultDto = _mapper.Map<GetProductDto>(product);

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            foreach (var image in resultDto.Images)
            {
                image.Url = $"{baseUrl}{image.Url}";
            }

            return resultDto;
        }

        private void ValidateImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Empty file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException($"Invalid file type: {extension}. Allowed: jpg, jpeg, png, webp.");

            if (file.Length > MaxFileSize)
                throw new ArgumentException($"File '{file.FileName}' exceeds 5MB limit.");
        }

        public async Task<List<GetCategoriesDto>> GetCategoriesAsync()
        {
            var categories = await _productRepository.GetAllCategoriesAsync();
            return _mapper.Map<List<GetCategoriesDto>>(categories);

        }

        public async Task<List<SearchSuggestionDto>> GetSearchSuggestion(string searchSuggestion)
        {
            var data = await _productRepository.GetProductsforSuggestion(searchSuggestion);
            var suggestions = data.Select(p => new SearchSuggestionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            }).ToList();
            return _mapper.Map<List<SearchSuggestionDto>>(suggestions);
        }

    }
}
