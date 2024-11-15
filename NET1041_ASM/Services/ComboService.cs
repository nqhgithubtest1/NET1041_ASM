using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class ComboService : IComboService
    {
        private readonly ApplicationDbContext _dbContext;
        public ComboService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ComboFoodItem> GetAllComboFoodItems()
        {
            return _dbContext.ComboFoodItems.ToList();
        }

        public List<Combo> GetAllCombos()
        {
            return _dbContext.Combos.ToList();
        }

        public Combo GetById(int id)
        {
            return _dbContext.Combos.FirstOrDefault(cb => cb.ComboID == id);
        }
    }
}
