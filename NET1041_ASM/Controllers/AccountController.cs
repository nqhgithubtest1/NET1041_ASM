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
                    TempData["ErrorMessage"] = "Registration failed. Please try again.";
                    return View(registerUser);
                }

                return View("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(registerUser);
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
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return View(loginUser);
                }

                loginUser = _accountService.GetByUsername(loginUser.Username);

                HttpContext.Session.SetString("Username", loginUser.Username);
                HttpContext.Session.SetString("UserID", loginUser.UserID.ToString());
                HttpContext.Session.SetString("UserRole", loginUser.Role);

                if (loginUser.Role == "customer")
                {
                    return RedirectToAction("Index", "Food");
                }
                if (loginUser.Role == "admin")
                {
                    return Redirect("/Admin/Home/Index");
                }

                TempData["ErrorMessage"] = "Something happening.";
                return View(loginUser);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(loginUser);
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("Username");
                HttpContext.Session.Remove("UserID");
                HttpContext.Session.Remove("UserRole");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }
    }
}
