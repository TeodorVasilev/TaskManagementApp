using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentApp.DAL.ViewModels.User;
using TaskManagmentApp.Service.UserService;

namespace TaskManagmentApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var model = await this._userService.LoadUserList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await this._userService.LoadRolesList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StoreUserViewModel formData)
        {
            var (result, message) = await this._userService.Create(formData);

            if (result)
            {
                ViewBag.Message = message;
                return RedirectToAction("Success", "User", new {message = message});
            }
            else
            {
                return RedirectToAction("Create", "User", new { message = message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this._userService.LoadEditForm(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(StoreEditUserViewModel formData)
        {
            var (result, message) = await this._userService.Update(formData);

            if (result)
            {
                return RedirectToAction("Success", "User", new { message = message });
            }
            else
            {
                return RedirectToAction("Edit", "User", new {id = formData.Id, message = message});
            }
        }

        public IActionResult Success(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var (result, message) = await this._userService.Delete(id);

            if (result)
            {
                return RedirectToAction("Success", "User", new { message = message });
            }
            else
            {
                return RedirectToAction("Index", "User", new { message = message });
            }
        }
    }
}
