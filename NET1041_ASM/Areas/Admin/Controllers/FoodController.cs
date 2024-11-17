using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET1041_ASM.Areas.Admin.Services;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly IAdminFoodService _foodService;
        private readonly IAdminCategoryService _categoryService;

        public FoodController(IAdminFoodService foodService, IAdminCategoryService categoryService)
        {
            _foodService = foodService;
            _categoryService = categoryService;
        }

        public IActionResult Index([FromQuery] FoodItemFilterViewModel filter)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                ViewData["PageTitle"] = "Food Menu";

                ViewBag.Categories = _categoryService.GetAll();

                var query = _foodService.GetAll().AsQueryable();

                if (filter.CategoryID.HasValue)
                {
                    query = query.Where(f => f.CategoryID == filter.CategoryID);
                }

                ViewBag.Categories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                });

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

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            ViewBag.Categories = _categoryService.GetAll()
            .Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.Name
            });
            return View();
        }

        [HttpPost]
        public IActionResult Create(FoodItem food, IFormFile imageFile)
        {
            ViewBag.Categories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                });

            food.CreatedDate = DateTime.Now;

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileExtension = Path.GetExtension(imageFile.FileName);

                    var fileName = $"{food.Name}{fileExtension}";

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/foods");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    food.ImagePath = $"img/foods/{fileName}";
                }

                _foodService.Add(food);

                TempData["SuccessMessage"] = "Food added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(food);
            }
        }

        public IActionResult Edit(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            var food = _foodService.GetById(id);
            if (food == null)
            {
                ViewData["ErrorMessage"] = "Not found content of this page.";
                return View("Error", "Shared");
            }
            ViewBag.Categories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                });
            return View(food);
        }

        [HttpPost]
        public IActionResult Edit(FoodItem food, IFormFile imageFile)
        {
            var currentFood = _foodService.GetById(food.FoodItemID);

            ViewBag.Categories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                });

            food.CreatedDate = DateTime.Now;

            try
            {
                // If a new image file is uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileExtension = Path.GetExtension(imageFile.FileName);
                    var newFileName = $"{food.Name}{fileExtension}";
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/foods", newFileName);

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(food.ImagePath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", food.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Save the new file
                    using (var fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    // Update the image path
                    food.ImagePath = $"img/foods/{newFileName}";
                }
                else
                {
                    food.ImagePath = currentFood.ImagePath;
                }

                _foodService.Update(food);

                TempData["SuccessMessage"] = "Food updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(food);
            }
        }

        [HttpPost]
        public IActionResult Deactive(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            _foodService.Deactivate(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Active(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            _foodService.Activate(id);
            return RedirectToAction("Index");
        }
    }
}
