using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET1041_ASM.Areas.Admin.Services;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IAdminOrderService _orderService;

        public OrderController(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index([FromQuery] OrderFilterViewModel filter)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            ViewBag.StatusOptions = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }).ToList();

            ViewBag.SortByOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "OrderID", Text = "Order ID" },
                new SelectListItem { Value = "OrderTime", Text = "Order Time" },
                new SelectListItem { Value = "TotalAmount", Text = "Total Amount" }
            };

            ViewBag.SortOrderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "asc", Text = "Ascending" },
                new SelectListItem { Value = "desc", Text = "Descending" }
            };

            try
            {
                var query = _orderService.GetAllOrders().AsQueryable();

                if (filter.OrderID.HasValue)
                {
                    query = query.Where(o => o.OrderID == filter.OrderID.Value);
                }

                if (filter.MinPrice.HasValue)
                {
                    query = query.Where(o => o.TotalAmount >= filter.MinPrice.Value);
                }
                if (filter.MaxPrice.HasValue)
                {
                    query = query.Where(o => o.TotalAmount <= filter.MaxPrice.Value);
                }

                if (filter.FromDate.HasValue)
                {
                    query = query.Where(o => o.OrderTime >= filter.FromDate.Value);
                }
                if (filter.ToDate.HasValue)
                {
                    query = query.Where(o => o.OrderTime <= filter.ToDate.Value);
                }

                if (!string.IsNullOrEmpty(filter.Status))
                {
                    var statusEnum = Enum.Parse<OrderStatus>(filter.Status);
                    query = query.Where(o => o.Status == statusEnum);
                }

                if (!string.IsNullOrEmpty(filter.SortBy))
                {
                    query = filter.SortBy.ToLower() switch
                    {
                        "ordertime" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(o => o.OrderTime) : query.OrderBy(o => o.OrderTime),
                        "totalamount" => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(o => o.TotalAmount) : query.OrderBy(o => o.TotalAmount),
                        _ => filter.SortOrder.ToLower() == "desc" ? query.OrderByDescending(o => o.OrderID) : query.OrderBy(o => o.OrderID)
                    };
                }

                var totalItems = query.Count();
                var orders = query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);
                ViewBag.CurrentPage = filter.Page;

                filter.Orders = orders;

                return View(filter);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult Details(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                ViewData["ErrorMessage"] = "Not found content of this page.";
                return View("Error", "Shared");
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Cancel(int orderId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                _orderService.CancelOrder(orderId);
                TempData["SuccessMessage"] = "Order has been successfully cancelled.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        [HttpPost]
        public IActionResult UpdateNextStatus(int orderId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "admin")
            {
                ViewData["ErrorMessage"] = "You do not have admin permission to access this page.";
                return View("Error", "Shared");
            }

            try
            {
                _orderService.UpdateNextStatus(orderId);
                TempData["SuccessMessage"] = "Order has been successfully updated.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }
    }
}
