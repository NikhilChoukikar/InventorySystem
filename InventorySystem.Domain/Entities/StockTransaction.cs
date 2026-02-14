using InventorySystem.Domain.Common;
using InventorySystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InventorySystem.Domain.Entities
{
    public class StockTransaction : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public StockType StockType { get; set; }
    }

}



