using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public interface IComboService
    {
        List<Combo> GetAllCombos();
        Combo GetById(int id);
        List<ComboFoodItem> GetAllComboFoodItems();
    }
}
