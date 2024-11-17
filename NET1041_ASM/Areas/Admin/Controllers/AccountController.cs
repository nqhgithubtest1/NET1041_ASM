using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET1041_ASM.Areas.Admin.Models;
using NET1041_ASM.Areas.Admin.Services;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAdminAccountService _accountService;

        public AccountController(IAdminAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index([FromQuery] AccountFilterViewModel filter)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            ViewBag.CurrentAdminId = int.Parse(HttpContext.Session.GetString("UserID"));
            var query = _accountService.GetAllUsers().AsQueryable();
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(a =>
                    a.Username.Contains(filter.Keyword) ||
                    a.Email.Contains(filter.Keyword) ||
                    a.Phone.Contains(filter.Keyword));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(a => a.IsActive == filter.IsActive.Value);
            }

            if (!string.IsNullOrEmpty(filter.Role))
            {
                query = query.Where(a => a.Role == filter.Role);
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "username" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(a => a.Username) : query.OrderBy(a => a.Username),
                    "email" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(a => a.Email) : query.OrderBy(a => a.Email),
                    "dateofbirth" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(a => a.DateOfBirth) : query.OrderBy(a => a.DateOfBirth),
                    _ => query.OrderBy(a => a.UserID)
                };
            }
            var totalItems = query.Count();
            var accounts = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            filter.Accounts = accounts;

            ViewBag.StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Active", Value = "true", Selected = filter.IsActive == true },
                new SelectListItem { Text = "Inactive", Value = "false", Selected = filter.IsActive == false }
            };

            ViewBag.RoleOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Customer", Value = "customer", Selected = filter.Role == "customer" },
                new SelectListItem { Text = "Admin", Value = "admin", Selected = filter.Role == "admin" }
            };

            ViewBag.SortByOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Username", Value = "username", Selected = filter.SortBy == "username" },
                new SelectListItem { Text = "Email", Value = "email", Selected = filter.SortBy == "email" },
                new SelectListItem { Text = "Date of Birth", Value = "dateofbirth", Selected = filter.SortBy == "dateofbirth" }
            };

            ViewBag.SortOrderOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ascending", Value = "asc", Selected = filter.SortOrder == "asc" },
                new SelectListItem { Text = "Descending", Value = "desc", Selected = filter.SortOrder == "desc" }
            };

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);
            ViewBag.CurrentPage = filter.Page;

            return View(filter);
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            ViewBag.RoleOptions = new SelectList(new[]
            {
                new { Value = "customer", Text = "Customer" },
                new { Value = "admin", Text = "Admin" }
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            ViewBag.RoleOptions = new SelectList(new[]
            {
                new { Value = "customer", Text = "Customer" },
                new { Value = "admin", Text = "Admin" }
            }, "Value", "Text");

            try
            {
                _accountService.AddUser(user);
                TempData["SuccessMessage"] = "User added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(user);
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

            var user = _accountService.GetUserById(id);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Not found content of this page.";
                return View("Error", "Shared");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            try
            {
                _accountService.UpdateUser(user);
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(user);
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

            var currentUser = int.Parse(HttpContext.Session.GetString("UserID"));
            if (currentUser == id)
            {
                TempData["ErrorMessage"] = "Cannot deactive the currently logged-in admin.";
                return RedirectToAction(nameof(Index));
            }

            _accountService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
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

            _accountService.ActiveUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
