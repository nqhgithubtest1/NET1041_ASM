using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public interface ICartService
    {
        Cart GetByUserID(int id);
        void AddToCart(int userId, int foodItemId, int quantity);
        void RemoveCartItem(int cartItemId);
        void UpdateCartItemQuantity(int cartItemId, int quantity);
    }
}
