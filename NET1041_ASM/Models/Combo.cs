using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class Combo
    {
        public int ComboID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(200)]
        public string ImagePath { get; set; }

        public bool IsAvailable { get; set; }

        // Lazy loading
        public virtual ICollection<ComboFoodItem> ComboFoodItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
