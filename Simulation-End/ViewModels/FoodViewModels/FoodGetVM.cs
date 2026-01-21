using System.ComponentModel.DataAnnotations;

namespace Simulation_End.ViewModels.FoodViewModels
{
    public class FoodGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }


    }
}
