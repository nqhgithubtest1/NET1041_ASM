using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using NET1041_ASM.Models;
using NET1041_ASM.Services;

namespace NET1041_ASM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IAccountService _accountService;

        public CustomerController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Profile()
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");

                if (string.IsNullOrEmpty(username))
                {
                    ViewData["ErrorMessage"] = "You must be logged in to access the profile.";
                    return View("Error");
                }

                var user = _accountService.GetByUsername(username);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = "User not found.";
                    return View("Error");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }

        [HttpPost]
        public IActionResult Profile([FromForm] User updateUser)
        {
            try
            {
                var user = _accountService.GetByUsername(updateUser.Username);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = "User not found.";
                    return View("Error");
                }

                user.Password = updateUser.Password;
                user.Email = updateUser.Email;
                user.Phone = updateUser.Phone;
                user.DateOfBirth = updateUser.DateOfBirth;

                _accountService.Update(user);

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error", "Shared");
            }
        }
    }
}
