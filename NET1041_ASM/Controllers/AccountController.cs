using Microsoft.AspNetCore.Mvc;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] User registerUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(registerUser);
            }

            if (!_accountService.Register(registerUser))
            {
                return View(registerUser);
            }

            return View("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] User loginUser)
        {
            if (!_accountService.Login(loginUser.Username, loginUser.Password))
            {
                return View(loginUser);
            }

            HttpContext.Session.SetString("Username", loginUser.Username);
            HttpContext.Session.SetString("UserRole", loginUser.Role);

            return RedirectToAction("Index", "Food");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Index", "Food");
        }
    }
}
