using System.ComponentModel.DataAnnotations;

namespace NET1041_ASM.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; } = "customer";

        public DateTime DateOfBirth { get; set; }

        // Lazy loading
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
