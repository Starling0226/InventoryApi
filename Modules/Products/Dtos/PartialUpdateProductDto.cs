using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Modules.Products.Dtos
{
    public class PartialUpdateProductDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string? Name { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Description { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int? Stock { get; set; }
    }
}