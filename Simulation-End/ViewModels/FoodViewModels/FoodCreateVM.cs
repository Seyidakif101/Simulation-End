using System.ComponentModel.DataAnnotations;

namespace Simulation_End.ViewModels.FoodViewModels
{
    public class FoodCreateVM
    {
        [Required,MaxLength(256),MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public IFormFile Image { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        [Required,Range(0,double.MaxValue)]
        public decimal Price { get; set; }
    }
}
