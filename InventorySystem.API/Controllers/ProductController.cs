using InventorySystem.Application.DTOs.Auth;
using InventorySystem.Application.DTOs.Product;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InventorySystem.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products?pageNumber=1&pageSize=10
      
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _productService.GetProductsAsync(pageNumber, pageSize);
            return Ok(products);
        }

        // POST: api/products
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var result = await _productService.CreateProductAsync(dto);
            if (!result)
                return BadRequest("Unable to create product");

            return Ok("Product created successfully");
        }

        
    }
}

