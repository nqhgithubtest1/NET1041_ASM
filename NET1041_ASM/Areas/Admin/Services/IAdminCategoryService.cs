using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public interface IAdminCategoryService
    {
        List<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Deactivate(int id);
        void Activate(int id);
    }
}
