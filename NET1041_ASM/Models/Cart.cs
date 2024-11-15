using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class Cart
    {
        public int CartID { get; set; }

        [Required]
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
