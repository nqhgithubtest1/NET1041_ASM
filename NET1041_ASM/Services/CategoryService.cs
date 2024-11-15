using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Category> GetAll()
        {
            return _dbContext.Categories.ToList();
        }
    }
}
