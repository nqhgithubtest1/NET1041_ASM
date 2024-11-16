using Microsoft.EntityFrameworkCore;
using NET1041_ASM.Models;

namespace NET1041_ASM.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboFoodItem> ComboFoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComboFoodItem>()
                .HasKey(cfi => new { cfi.ComboID, cfi.FoodItemID });

            modelBuilder.Entity<ComboFoodItem>()
                .HasOne(cfi => cfi.Combo)
                .WithMany(c => c.ComboFoodItems)
                .HasForeignKey(cfi => cfi.ComboID);

            modelBuilder.Entity<ComboFoodItem>()
                .HasOne(cfi => cfi.FoodItem)
                .WithMany(fi => fi.ComboFoodItems)
                .HasForeignKey(cfi => cfi.FoodItemID);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.FoodItem)
                .WithMany(f => f.OrderDetails)
                .HasForeignKey(od => od.FoodItemID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Combo)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(od => od.ComboID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.FoodItem)
                .WithMany(fi => fi.CartItems)
                .HasForeignKey(ci => ci.FoodItemID);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Combo)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.ComboID);
        }
    }
}
