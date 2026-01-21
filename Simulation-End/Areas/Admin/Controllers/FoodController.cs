using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simulation_End.Context;
using Simulation_End.Helper;
using Simulation_End.Models;
using Simulation_End.ViewModels.FoodViewModels;

namespace Simulation_End.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FoodController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public FoodController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "images");
        }
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
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await _sendCategoryViewBag();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FoodCreateVM vm)
        {
            await _sendCategoryViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!existCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category yoxdu!!!");
                return View(vm);
            }
            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("Image", "Olcu 2mb kicik olmalidi!!!");
                return View(vm);
            }
            if (!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("Image", "Image tipinde olmalidi!!!");
                return View(vm);
            }
            string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);
            Food food = new()
            {
                Name=vm.Name,
                Description=vm.Description,
                CategoryId=vm.CategoryId,
                Price=vm.Price,
                ImagePath=uniqueFileName
            };
            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if(food is null)
            {
                return NotFound();
            }
            FoodUpdateVM vm = new()
            {
                Name = food.Name,
                Description = food.Description,
                CategoryId = food.CategoryId,
                Price = food.Price,
            };
            await _sendCategoryViewBag();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(FoodUpdateVM vm)
        {
            await _sendCategoryViewBag();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!existCategory)
            {
                ModelState.AddModelError("CategoryId", "Bele bir category yoxdu!!!");
                return View(vm);
            }
            if (!vm.Image?.CheckSize(2)??false)
            {
                ModelState.AddModelError("Image", "Olcu 2mb kicik olmalidi!!!");
                return View(vm);
            }
            if (!vm.Image?.CheckType("image")??false)
            {
                ModelState.AddModelError("Image", "Image tipinde olmalidi!!!");
                return View(vm);
            }
            var existFood = await _context.Foods.FindAsync(vm.Id);

            if (existFood is null)
                return BadRequest();


            existFood.Name = vm.Name;
            existFood.Description = vm.Description;
            existFood.Price = vm.Price;
            existFood.CategoryId = vm.CategoryId;
            if(vm.Image is { })
            {
                string uniqueFileName = await vm.Image.FileUploadAsync(_folderPath);
                string olderFileName = Path.Combine(_folderPath, existFood.ImagePath);
                FileHelper.FileDelete(olderFileName);
                existFood.ImagePath = uniqueFileName;
            }
            _context.Foods.Update(existFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var folder = await _context.Foods.FindAsync(id);
            if(folder is null)
            {
                return NotFound();

            }
            _context.Foods.Remove(folder);
            await _context.SaveChangesAsync();
            string deleteFileName = Path.Combine(_folderPath, folder.ImagePath);
            FileHelper.FileDelete(deleteFileName);
            return RedirectToAction(nameof(Index));
        }
        private async Task _sendCategoryViewBag()
        {
            var categories = await _context.Categories.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name

            }).ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
