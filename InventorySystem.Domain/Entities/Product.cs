using InventorySystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Entities
{

    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}