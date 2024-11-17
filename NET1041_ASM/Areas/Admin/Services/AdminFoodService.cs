using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public class AdminFoodService : IAdminFoodService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminFoodService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FoodItem> GetAll()
        {
            return _dbContext.FoodItems.ToList();
        }

        public FoodItem GetById(int id)
        {
            return _dbContext.FoodItems.FirstOrDefault(f => f.FoodItemID == id);
        }

        public void Add(FoodItem food)
        {
            ValidateFood(food);
            _dbContext.FoodItems.Add(food);
            _dbContext.SaveChanges();
        }

        public void Update(FoodItem food)
        {
            ValidateFood(food);

            var existingFood = _dbContext.FoodItems.FirstOrDefault(f => f.FoodItemID == food.FoodItemID);
            if (existingFood != null)
            {
                existingFood.Name = food.Name;
                existingFood.Description = food.Description;
                existingFood.Price = food.Price;
                existingFood.CategoryID = food.CategoryID;
                existingFood.ImagePath = food.ImagePath;
                existingFood.IsAvailable = food.IsAvailable;
                _dbContext.SaveChanges();
            }
        }

        private void ValidateFood(FoodItem food)
        {
            if (string.IsNullOrEmpty(food.Name))
            {
                throw new ArgumentException("Food name is required.");
            }

            if (food.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            if (food.CategoryID <= 0)
            {
                throw new ArgumentException("A valid category must be selected.");
            }

            var isDuplicateName = _dbContext.FoodItems
                .Any(f => f.Name == food.Name && f.FoodItemID != food.FoodItemID);

            if (isDuplicateName)
            {
                throw new ArgumentException("Food name already exists. Please choose a different name.");
            }

            // Validate image for create and update
            if (food.ImagePath == null)
            {
                throw new ArgumentException("An image is required for the food item.");
            }
        }

        public void Deactivate(int id)
        {
            var food = _dbContext.FoodItems.FirstOrDefault(f => f.FoodItemID == id);
            if (food != null)
            {
                food.IsAvailable = false;
                _dbContext.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            var food = _dbContext.FoodItems.FirstOrDefault(f => f.FoodItemID == id);
            if (food != null)
            {
                food.IsAvailable = true;
                _dbContext.SaveChanges();
            }
        }
    }
}
