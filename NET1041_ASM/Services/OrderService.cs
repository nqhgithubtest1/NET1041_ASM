using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CreateOrder(int userId)
        {
            var cart = _dbContext.Carts
                .FirstOrDefault(c => c.UserID == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Your cart is empty.");
            }

            var order = new Order
            {
                UserID = userId,
                OrderTime = DateTime.Now,
                Status = OrderStatus.Pending,
                TotalAmount = cart.CartItems.Sum(ci => ci.Price * ci.Quantity),
                OrderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    FoodItemID = ci.FoodItemID,
                    ComboID = ci.ComboID,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            _dbContext.Orders.Add(order);

            _dbContext.CartItems.RemoveRange(cart.CartItems);
            _dbContext.SaveChanges();

            return order.OrderID;
        }

        public Order GetOrderDetails(int orderId)
        {
            var order = _dbContext.Orders
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return order;
        }

        public List<Order> GetOrderHistory(int userId)
        {
            return _dbContext.Orders
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderTime)
                .ToList();
        }

        public void CancelOrder(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be cancelled.");
            }

            order.Status = OrderStatus.Cancelled;
            _dbContext.SaveChanges();
        }
    }
}
