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
            var username = HttpContext.Session.GetString("Username");
            var user = _accountService.GetByUsername(username);
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile([FromForm] User updateUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(updateUser);
            }

            var user = _accountService.GetByUsername(updateUser.Username);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = updateUser.Password;
            user.Email = updateUser.Email;
            user.Phone = updateUser.Phone;
            user.DateOfBirth = updateUser.DateOfBirth;

            _accountService.Update(user);

            return RedirectToAction("Index", "Food");
        }
    }
}
