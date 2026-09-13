using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IproductImageService _imageService;

        public ProductImageController(IproductImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int productId, IFormFile file, [FromQuery] bool isMain = false)
        {
            try
            {
                var result = await _imageService.UploadImageAsync(productId, file, isMain);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int productId)
        {
            var images = await _imageService.GetImagesAsync(productId);
            return Ok(images);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int productId, int imageId)
        {
            try
            {
                await _imageService.DeleteImageAsync(imageId);
                return Ok("image deleted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
