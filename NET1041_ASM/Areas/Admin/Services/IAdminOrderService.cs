using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public interface IAdminOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        void CancelOrder(int orderId);
        void UpdateNextStatus(int orderId);
    }
}
