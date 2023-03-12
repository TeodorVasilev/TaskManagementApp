using Microsoft.AspNetCore.Mvc;
using TaskManagmentApp.DAL.Data;
using TaskManagmentApp.DAL.ViewModels.Account;
using TaskManagmentApp.Service.LoginService;

namespace TaskManagmentApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly AppDbContext _context;
        public AccountController(ILoginService loginService, AppDbContext context)
        {
            this._loginService = loginService;  
            this._context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            var result = await this._loginService.LoginAttempt(formData);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Wrong username or password";
                return View();
            }
        }
        
        public async Task<IActionResult> Logout()
        {
            this._loginService.Logout();
            return RedirectToAction("Login", "Account");
        }
    }
}
