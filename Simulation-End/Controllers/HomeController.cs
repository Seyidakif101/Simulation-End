using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation_End.Context;
using Simulation_End.ViewModels.FoodViewModels;

namespace Simulation_End.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var foods = await _context.Foods.Select(x => new FoodGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name,
                Price = x.Price
            }).ToListAsync();
            return View(foods);
        }
    }
}
