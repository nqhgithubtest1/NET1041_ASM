using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Order()
        {
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
            try
            {
                var order = _orderService.GetOrderDetails(id);

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

        public IActionResult History()
        {
            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserID"));
                var orders = _orderService.GetOrderHistory(userId);

                if (orders == null || !orders.Any())
                {
                    ViewBag.Message = "You have no orders yet.";
                }

                return View(orders);
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
