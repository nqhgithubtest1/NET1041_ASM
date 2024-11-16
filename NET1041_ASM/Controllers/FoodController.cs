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
            try
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
                    query = filter.SortBy.ToLower() switch
                    {
                        "price" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(f => f.Price) : query.OrderBy(f => f.Price),
                        _ => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(f => f.Name) : query.OrderBy(f => f.Name)
                    };
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
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult Detail(int id)
        {
            try
            {
                var foodItem = _foodService.GetById(id);

                if (foodItem == null)
                {
                    ViewData["ErrorMessage"] = $"Food item with ID {id} not found.";
                    return View("Error");
                }

                return View(foodItem);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }
    }
}
