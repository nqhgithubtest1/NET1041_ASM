using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminCategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Category> GetAll()
        {
            return _dbContext.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
        }

        public void Add(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                _dbContext.SaveChanges();
            }
        }

        public void Deactivate(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category != null)
            {
                category.IsActive = false;
                _dbContext.SaveChanges();
            }
        }

        public void Activate(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.CategoryID == id);
            if (category != null)
            {
                category.IsActive = true;
                _dbContext.SaveChanges();
            }
        }
    }
}
