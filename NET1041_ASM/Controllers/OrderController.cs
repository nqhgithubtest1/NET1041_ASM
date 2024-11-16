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
                return RedirectToAction("Details", "Order", new { id = orderId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while placing the order.";
                return RedirectToAction("Index", "Cart");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var order = _orderService.GetOrderDetails(id);
                return View(order);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching order details.";
                return RedirectToAction("Index", "Home");
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
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching order history.";
                return RedirectToAction("Index", "Food");
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
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id = orderId });
            }
        }
    }
}
