using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class FoodService : IFoodService
    {
        private readonly ApplicationDbContext _dbContext;
        public FoodService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<FoodItem> GetAll()
        {
            return _dbContext.FoodItems.ToList();
        }

        public FoodItem GetById(int id)
        {
            return _dbContext.FoodItems.FirstOrDefault(fi => fi.FoodItemID == id);
        }
    }
}
