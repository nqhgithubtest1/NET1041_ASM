using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public interface IAdminAccountService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        void ActiveUser(int id);
    }
}
