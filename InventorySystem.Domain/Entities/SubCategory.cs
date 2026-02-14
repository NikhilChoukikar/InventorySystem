using InventorySystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
    }

}



