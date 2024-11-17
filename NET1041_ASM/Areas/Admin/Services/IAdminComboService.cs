using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public interface IAdminComboService
    {
        List<Combo> GetAll();
        Combo GetById(int id);
        void Add(Combo combo);
        void Update(Combo combo);
        void Deactivate(int id);
        void Activate(int id);
    }
}
