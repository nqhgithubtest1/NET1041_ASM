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
            try
            {
                if (!_accountService.Register(registerUser))
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                    return View(registerUser);
                }

                return View("Login");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] User loginUser)
        {
            try
            {
                if (!_accountService.Login(loginUser.Username, loginUser.Password))
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(loginUser);
                }

                loginUser = _accountService.GetByUsername(loginUser.Username);

                HttpContext.Session.SetString("Username", loginUser.Username);
                HttpContext.Session.SetString("UserID", loginUser.UserID.ToString());
                HttpContext.Session.SetString("UserRole", loginUser.Role);

                return RedirectToAction("Index", "Food");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("Username");
                HttpContext.Session.Remove("UserRole");
                return RedirectToAction("Index", "Food");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }
    }
}
