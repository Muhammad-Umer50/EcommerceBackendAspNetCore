using ECommerceStore.Dtos.ProductsDtos;
using ECommerceStore.Models;
using ECommerceStore.RequestParameters;
using ECommerceStore.Services;
using ECommerceStore.Services.Interfaces;
using ECommerceStore.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
            
        }
        [HttpGet("/Products")]
        public async Task<ActionResult<PagedResult<GetProductDto>>> GetProducts(
            [FromQuery] ProductQueryParameters query)
        {
            var products = await _service.GetProductsAsync(query);
            return Ok(products);
        }
        [HttpPost("/Products")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GetProductDto>> CreateProduct([FromForm] CreateProductDto dto)
        {
            try
            {
                var result = await _service.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetProducts), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/SearchSuggestions")]
        public async Task<ActionResult<List<SearchSuggestionDto>>> GetProductSuggestions([FromQuery] string searchSuggestion)
        {
            var result = await _service.GetSearchSuggestion(searchSuggestion);
            return Ok(result);
        }
        [HttpGet("/Products/{id}")]
        public async Task<ActionResult<GetProductDto>> GetProductById(int id)
        {
            try
            {
                var product = await _service.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
