using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Range(0, 100000)]
        public decimal TotalAmount { get; set; }

        // Quan hệ
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
