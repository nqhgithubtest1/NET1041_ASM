using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _dbContext;
        public CartService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddComboToCart(int userId, int comboId, int quantity)
        {
            var userCart = _dbContext.Carts.FirstOrDefault(c => c.UserID == userId);

            if (userCart == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }

            var combo = _dbContext.Combos.Find(comboId);

            if (combo == null || quantity <= 0)
            {
                throw new ArgumentException("Invalid combo or quantity.");
            }

            var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.ComboID == comboId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.Price = combo.Price;
            }
            else
            {
                userCart.CartItems.Add(new CartItem
                {
                    ComboID = comboId,
                    Quantity = quantity,
                    Price = combo.Price
                });
            }

            _dbContext.SaveChanges();
        }

        public void AddToCart(int userId, int foodItemId, int quantity)
        {
            var userCart = _dbContext.Carts.FirstOrDefault(c => c.UserID == userId);

            if (userCart == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }

            var foodItem = _dbContext.FoodItems.Find(foodItemId);

            if (foodItem == null || quantity <= 0)
            {
                throw new ArgumentException("Invalid food item or quantity.");
            }

            var cartItem = userCart.CartItems.FirstOrDefault(ci => ci.FoodItemID == foodItemId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.Price = foodItem.Price;
            }
            else
            {
                userCart.CartItems.Add(new CartItem
                {
                    FoodItemID = foodItemId,
                    Quantity = quantity,
                    Price = foodItem.Price
                });
            }

            _dbContext.SaveChanges();
        }

        public Cart GetByUserID(int id)
        {
            return _dbContext.Carts.FirstOrDefault(c => c.UserID == id);
        }

        public void RemoveCartItem(int cartItemId)
        {
            var cartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.CartItemID == cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            _dbContext.CartItems.Remove(cartItem);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var cartItem = _dbContext.CartItems
                .FirstOrDefault(ci => ci.CartItemID == cartItemId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found.");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.");
            }

            cartItem.Quantity = quantity;

            if (cartItem.FoodItemID != null)
            {
                cartItem.Price = cartItem.FoodItem.Price;
            }
            else if (cartItem.ComboID != null)
            {
                cartItem.Price = cartItem.Combo.Price;
            }

            _dbContext.SaveChanges();
        }
    }
}
