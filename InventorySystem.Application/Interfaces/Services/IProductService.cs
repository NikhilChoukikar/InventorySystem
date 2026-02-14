using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::InventorySystem.Application.DTOs.Product;
using InventorySystem.Application.DTOs.Product;

namespace InventorySystem.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetProductsAsync(int pageNumber, int pageSize);
        Task<bool> CreateProductAsync(CreateProductDto dto);
    }
}
