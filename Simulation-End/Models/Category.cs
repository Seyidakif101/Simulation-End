using Simulation_End.Models.Common;

namespace Simulation_End.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Food> Foods { get; set; } = [];
    }
}
