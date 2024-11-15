using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public interface IFoodService
    {
        List<FoodItem> GetAll();
        FoodItem GetById(int id);
    }
}
