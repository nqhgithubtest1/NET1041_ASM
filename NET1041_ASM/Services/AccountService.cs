﻿using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Login(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                throw new Exception("User is not exists.");
            }

            if (!password.Equals(user.Password))
            {
                throw new Exception("Password is not true.");
            }

            return true;
        }

        public bool Register(User registerUser)
        {
            if (_dbContext.Users.Any(u => u.Username == registerUser.Username))
            {
                throw new Exception("Username already exists.");
            }

            if (_dbContext.Users.Any(u => u.Email == registerUser.Email))
            {
                throw new Exception("Email already exists.");
            }

            _dbContext.Users.Add(registerUser);
            _dbContext.SaveChanges();

            return true;
        }
    }
}