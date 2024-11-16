using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public interface IOrderService
    {
        int CreateOrder(int userId);
        Order GetOrderDetails(int orderId);
        List<Order> GetOrderHistory(int userId);
        void CancelOrder(int orderId);
    }
}
