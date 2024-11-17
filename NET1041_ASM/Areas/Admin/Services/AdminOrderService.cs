using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Context;
using NET1041_ASM.Models;
using System;

namespace NET1041_ASM.Areas.Admin.Services
{
    public class AdminOrderService : IAdminOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminOrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> GetAllOrders()
        {
            return _dbContext.Orders
                .OrderByDescending(o => o.OrderTime)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _dbContext.Orders
                .FirstOrDefault(o => o.OrderID == id);
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

        public void UpdateNextStatus(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            if (order.Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException("Completed order can not be processed anymore.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Cancelled order can not be processed anymore.");
            }

            if (order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Shipping;
            }

            else if (order.Status == OrderStatus.Shipping)
            {
                order.Status = OrderStatus.Completed;
            }

            _dbContext.SaveChanges();
        }
    }
}
