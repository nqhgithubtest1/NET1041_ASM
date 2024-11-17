using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminAccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.UserID == id);
        }

        private void ValidateUser(User user)
        {
            if (_dbContext.Users.Any(u => u.Username == user.Username && u.UserID != user.UserID))
                throw new InvalidOperationException($"Username '{user.Username}' already exists.");

            if (_dbContext.Users.Any(u => u.Email == user.Email && u.UserID != user.UserID))
                throw new InvalidOperationException($"Email '{user.Email}' already exists.");

            if (_dbContext.Users.Any(u => u.Phone == user.Phone && u.UserID != user.UserID))
                throw new InvalidOperationException($"Phone number '{user.Phone}' already exists.");

            if (user.Role != "customer" && user.Role != "admin")
            {
                throw new InvalidOperationException($"The role '{user.Role}' is invalid.");
            }
        }

        public void AddUser(User user)
        {
            ValidateUser(user);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            ValidateUser(user);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                user.IsActive = false;
                _dbContext.SaveChanges();
            }
        }

        public void ActiveUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserID == id);
            if (user != null)
            {
                user.IsActive = true;
                _dbContext.SaveChanges();
            }
        }
    }
}
