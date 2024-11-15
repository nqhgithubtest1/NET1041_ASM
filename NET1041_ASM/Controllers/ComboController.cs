using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class ComboController : Controller
    {
        private readonly IComboService _comboService;
        public ComboController(IComboService comboService)
        {
            _comboService = comboService; 
        }
        public IActionResult Index([FromQuery] ComboFilterViewModel filter)
        {
            ViewData["PageTitle"] = "Combo Menu";

            var query = _comboService.GetAllCombos().AsQueryable();

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
            var combos = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);
            ViewBag.CurrentPage = filter.Page;

            filter.Combos = combos;

            return View(filter);
        }
        
        public IActionResult Detail(int id)
        {
            var combo = _comboService.GetById(id);

            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }
    }
}
