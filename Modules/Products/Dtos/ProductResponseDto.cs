namespace InventoryApi.Modules.Products.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        
        // Datos planos de la categoría
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}