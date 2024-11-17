using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET1041_ASM.Areas.Admin.Services;
using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComboController : Controller
    {
        private readonly IAdminComboService _comboService;
        private readonly IAdminFoodService _foodService;

        public ComboController(IAdminComboService comboService, IAdminFoodService foodService)
        {
            _comboService = comboService;
            _foodService = foodService;
        }

        public IActionResult Index([FromQuery] ComboFilterViewModel filter)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                ViewData["PageTitle"] = "Combo Menu";

                var query = _comboService.GetAll().AsQueryable();

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

            ViewBag.FoodItems = _foodService.GetAll().Where(f => f.IsAvailable == true).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Combo combo, IFormFile imageFile)
        {
            combo.ComboFoodItems = combo.ComboFoodItems
            .Where(item => item.Quantity > 0)
            .ToList();

            ViewBag.FoodItems = _foodService.GetAll().Where(f => f.IsAvailable == true).ToList();

            try
            {
                if (imageFile != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "combos");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileExtension = Path.GetExtension(imageFile.FileName);
                    var fileName = $"{combo.Name}{fileExtension}";

                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    combo.ImagePath = "img/combos/" + fileName;
                }

                _comboService.Add(combo);
                TempData["SuccessMessage"] = "Combo added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(combo);
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

            ViewBag.FoodItems = _foodService.GetAll().Where(f => f.IsAvailable == true).ToList();
            var combo = _comboService.GetById(id);
            if (combo == null)
            {
                ViewData["ErrorMessage"] = "Not found content of this page.";
                return View("Error", "Shared");
            }

            return View(combo);
        }

        [HttpPost]
        public IActionResult Edit(Combo combo, IFormFile imageFile)
        {
            var currentCombo = _comboService.GetById(combo.ComboID);

            try
            {
                // If a new image file is uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileExtension = Path.GetExtension(imageFile.FileName);
                    var newFileName = $"{combo.Name}{fileExtension}";
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/combos", newFileName);

                    // Delete old file if it exists
                    if (!string.IsNullOrEmpty(currentCombo.ImagePath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", currentCombo.ImagePath.TrimStart('/'));
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
                    combo.ImagePath = $"img/combos/{newFileName}";
                }
                else
                {
                    // Retain the old image path if no new file is uploaded
                    combo.ImagePath = currentCombo.ImagePath;
                }

                // Update the combo in the database
                _comboService.Update(combo);

                TempData["SuccessMessage"] = "Combo updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(combo);
            }
        }

        [HttpPost]
        public IActionResult Deactivate(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                _comboService.Deactivate(id);
                TempData["SuccessMessage"] = "Combo deactivated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Activate(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                _comboService.Activate(id);
                TempData["SuccessMessage"] = "Combo activated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
