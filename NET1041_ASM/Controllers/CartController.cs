using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IFoodService _foodService;
        public CartController(ICartService cartService, IFoodService foodService)
        {
            _cartService = cartService;
            _foodService = foodService;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserID"));
            var userCart = _cartService.GetByUserID(userId);

            if (userCart == null || userCart.CartItems.Count == 0)
            {
                ViewBag.Message = "Your cart is empty.";
            }

            return View(userCart);
        }

        [HttpPost]
        public IActionResult AddToCart(int foodItemId, int quantity)
        {
            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserID"));
                _cartService.AddToCart(userId, foodItemId, quantity);
                return RedirectToAction("Index", "Cart");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Remove(int CartItemID)
        {
            try
            {
                _cartService.RemoveCartItem(CartItemID);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int CartItemID, int Quantity)
        {
            try
            {
                _cartService.UpdateCartItemQuantity(CartItemID, Quantity);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
