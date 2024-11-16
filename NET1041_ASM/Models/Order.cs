using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public enum OrderStatus
    {
        Pending,
        Shipping,
        Completed,
        Cancelled
    }
    public class Order
    {
        public int OrderID { get; set; }

        public DateTime OrderTime { get; set; }

        [Required]
        [MaxLength(50)]
        public OrderStatus Status { get; set; }

        [Range(0, 100000)]
        public decimal TotalAmount { get; set; }

        // Relationship
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
