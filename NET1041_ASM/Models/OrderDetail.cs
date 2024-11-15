using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        // Quan hệ
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public int? FoodItemID { get; set; }
        public virtual FoodItem FoodItem { get; set; }
        public int? ComboID { get; set; }
        public virtual Combo Combo { get; set; }
    }
}
