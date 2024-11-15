using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public interface IAccountService
    {
        bool Login(string username, string password);
        bool Register(User registerUser);
        void CreateCartForUser(int userId);
        User GetByUsername(string username);
        void Update(User user);
    }
}
