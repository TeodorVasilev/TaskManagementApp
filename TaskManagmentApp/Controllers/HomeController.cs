using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentApp.Service.HomeService;

namespace TaskManagmentApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this._homeService.LoadHome();
            return View(model);
        }
    }
}