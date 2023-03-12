using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagmentApp.DAL.ViewModels.Assignment;
using TaskManagmentApp.Service.AssignmentService;

namespace TaskManagmentApp.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            this._assignmentService = assignmentService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<AssignmentViewModel> model = await this._assignmentService.LoadAssignmentList();
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Complete(int id)
        {
            this._assignmentService.Complete(id);
            return RedirectToAction("Index", "Assignment");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(await this._assignmentService.LoadCreateForm());
        }
        
        public IActionResult Success(string message)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id) 
        {
            return View(await this._assignmentService.LoadEditForm(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(StoreAssignmentViewModel formData)
        {
            var (result, message) = await this._assignmentService.Update(formData);

            if (result)
            {
                return RedirectToAction("Success", "Assignment", new { message = message });
            }
            else
            {
                return RedirectToAction("Edit", "Assignment", new {id = formData.Id, message = message});
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StoreAssignmentViewModel formData)
        {
            var (result, message) = await this._assignmentService.Create(formData);

            if (result)
            {
                return RedirectToAction("Success", "Assignment", new {message = message});
            }
            else
            {
                return RedirectToAction("Create", "Assignment", new { message = message });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await this._assignmentService.Delete(id);
            return RedirectToAction("Success", "Assignment", new {message = message});
        }
    }
}
