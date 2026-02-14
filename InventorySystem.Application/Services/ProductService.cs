using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::InventorySystem.Application.DTOs.Product;
using global::InventorySystem.Application.Interfaces.Repositories;
using global::InventorySystem.Application.Interfaces.Services;
using global::InventorySystem.Domain.Entities;


namespace InventorySystem.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetAllAsync(pageNumber, pageSize);

            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                Price = p.Price
            });
        }

        public async Task<bool> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            var id = await _productRepository.CreateAsync(product);
            return id > 0;
        }
    }

}
