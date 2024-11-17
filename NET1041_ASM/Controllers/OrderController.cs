using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult Order()
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Please login with customer account to access this page.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserID"));
                var orderId = _orderService.CreateOrder(userId);

                TempData["SuccessMessage"] = $"Order #{orderId} has been successfully placed!";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult Details(int id)
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Please login with customer account to access this page.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var order = _orderService.GetOrderDetails(id);

                if (!username.Equals(order.User.Username))
                {
                    throw new Exception($"You do not have permission to access the order with ID #{id}.");
                }

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found.");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult History([FromQuery] OrderFilterViewModel filter)
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Please login with customer account to access this page.";
                return RedirectToAction("Login", "Account");
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
                var userId = int.Parse(HttpContext.Session.GetString("UserID"));
                var query = _orderService.GetOrderHistory(userId).AsQueryable();

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

        [HttpPost]
        public IActionResult Cancel(int orderId)
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Please login with customer account to access this page.";
                return RedirectToAction("Login", "Account");
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
    }
}
