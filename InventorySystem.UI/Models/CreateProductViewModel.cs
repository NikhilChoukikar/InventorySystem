namespace InventorySystem.UI.Models
{
    public class CreateProductViewModel
    {
        public string Name { get; set; } = "";
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Error { get; set; }
    }
}
