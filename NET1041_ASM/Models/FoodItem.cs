using static Azure.Core.HttpHeader;
using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class FoodItem
    {
        public int FoodItemID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(500)]
        public string ImagePath { get; set; }

        // Relationship
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ComboFoodItem> ComboFoodItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
