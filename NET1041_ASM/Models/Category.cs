using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Lazy loading
        public virtual ICollection<FoodItem> FoodItems { get; set; }
    }
}
