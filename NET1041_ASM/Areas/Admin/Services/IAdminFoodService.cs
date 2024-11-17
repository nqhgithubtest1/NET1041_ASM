using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public interface IAdminFoodService
    {
        List<FoodItem> GetAll();
        FoodItem GetById(int id);
        void Add(FoodItem food);
        void Update(FoodItem food);
        void Deactivate(int id);
        void Activate(int id);
    }
}
