using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Modules.Categories.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
    }
}