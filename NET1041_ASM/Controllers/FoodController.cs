using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class FoodController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly ICategoryService _categoryService;
        public FoodController(IFoodService foodService, ICategoryService categoryService)
        {
            _foodService = foodService;
            _categoryService = categoryService;

        }
        public IActionResult Index([FromQuery] FoodItemFilterViewModel filter)
        {
            ViewData["PageTitle"] = "Food Menu";

            ViewBag.Categories = _categoryService.GetAll();

            var query = _foodService.GetAll().AsQueryable();

            if (filter.CategoryID.HasValue)
            {
                query = query.Where(f => f.CategoryID == filter.CategoryID.Value);
            }

            ViewBag.SortByOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "Name", Text = "Name" },
                new SelectListItem { Value = "Price", Text = "Price" }
            };

            ViewBag.SortOrderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "asc", Text = "Ascending" },
                new SelectListItem { Value = "desc", Text = "Descending" }
            };

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(f => f.Name.Contains(filter.Keyword) || f.Description.Contains(filter.Keyword));
            }

            if (filter.PriceFrom.HasValue)
            {
                query = query.Where(f => f.Price >= filter.PriceFrom.Value);
            }
            if (filter.PriceTo.HasValue)
            {
                query = query.Where(f => f.Price <= filter.PriceTo.Value);
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                if (filter.SortBy.ToLower() == "price")
                {
                    query = filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(f => f.Price) : query.OrderBy(f => f.Price);
                }
                else
                {
                    query = filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name);
                }
            }

            var totalItems = query.Count();
            var foodItems = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);
            ViewBag.CurrentPage = filter.Page;

            filter.FoodItems = foodItems;

            return View(filter);
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var foodItem = _foodService.GetById(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }
    }
}
