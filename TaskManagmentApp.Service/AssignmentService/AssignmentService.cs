using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagmentApp.DAL.Data;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.Models.Assignments;
using TaskManagmentApp.DAL.ViewModels.Assignment;
using TaskManagmentApp.DAL.ViewModels.User;
using TaskManagmentApp.Service.UserService;

namespace TaskManagmentApp.Service.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public AssignmentService(AppDbContext context, IUserService userService, UserManager<AppUser> userManager)
        {
            this._userService = userService;
            this._context = context;
            this._userManager = userManager;
        }

        public Assignment GetAssignmentById(int id)
        {
            return this._context.Assignments.Where(a => a.Id == id).FirstOrDefault();
        }

        public async Task<List<Assignment>> GetAllAssignments()
        {
            var userId = this._userService.GetUserId();
            var user = await this._userService.GetUserById(userId);

            var query = this._context.Assignments.AsQueryable();

            if(await this._userManager.IsInRoleAsync(user, "Employee"))
            {
                query = query.Where(a => a.UserId == user.Id);
            }

            var assignments = await query.ToListAsync();
            return assignments;
        }

        public async Task<List<AssignmentViewModel>> LoadAssignmentList()
        {
            var assignments = await this.GetAllAssignments();

            var list = assignments.Select(a => new AssignmentViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                CreatedAt = a.CreatedAt,
                DueDate = a.DueDate,
                Status = a.Status
            }).ToList();

            return list;
        }

        public async Task<List<UserViewModel>> LoadCreateForm()
        {
            var list = await this._userService.LoadUserList();
            return list;
        }

        public async Task<EditAssignmentViewModel> LoadEditForm(int id)
        {
            var assignment = this.GetAssignmentById(id);
            var assignmentModel = new AssignmentViewModel()
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                Status = assignment.Status,
                CreatedAt = assignment.CreatedAt,
                DueDate = assignment.DueDate,
                UserId = assignment.UserId
            };

            var model = new EditAssignmentViewModel();
            model.Users = await this._userService.LoadUserList();
            model.Assignment = assignmentModel;

            return model;
        }

        public async Task<(bool, string)> Create(StoreAssignmentViewModel formData)
        {
            if (formData.DueDate <= DateTime.Now.AddHours(1))
            {
                return (false, "Due date must be at least 1 hour from now.");
            }
            if (string.IsNullOrWhiteSpace(formData.Title) || formData.Title.Length < 3)
            {
                return (false, "Title must be at least 3 characters long.");
            }
            if (string.IsNullOrWhiteSpace(formData.Description) || formData.Description.Length < 10)
            {
                return (false, "Description must be at least 10 characters long.");
            }

            var assignment = new Assignment()
            {
                UserId = formData.UserId,
                Title = formData.Title,
                Description = formData.Description,
                CreatedAt = DateTime.Now,
                CompletedAt = DateTime.MinValue,
                DueDate = formData.DueDate,
                Status = AssignmentStatus.InProgress,
            };

            this._context.Assignments.Add(assignment);
            await this._context.SaveChangesAsync();

            return (true, "Assignment created successfully.");
        }

        public async Task<(bool, string)> Update(StoreAssignmentViewModel formData)
        {
            if (formData.DueDate <= DateTime.Now.AddHours(1))
            {
                return (false, "Due date must be at least 1 hour from now.");
            }
            if (string.IsNullOrWhiteSpace(formData.Title) || formData.Title.Length < 3)
            {
                return (false, "Title must be at least 3 characters long.");
            }
            if (string.IsNullOrWhiteSpace(formData.Description) || formData.Description.Length < 10)
            {
                return (false, "Description must be at least 10 characters long.");
            }

            var assignment = this.GetAssignmentById(formData.Id.Value);

            if (assignment.Title != formData.Title)
            {
                assignment.Title = formData.Title;
            }
            if (assignment.Description != formData.Description)
            {
                assignment.Description = formData.Description;
            }
            if (assignment.DueDate != formData.DueDate && formData.DueDate > assignment.DueDate)
            {
                assignment.DueDate = formData.DueDate;
            }
            if (assignment.Status != formData.Status)
            {
                assignment.Status = formData.Status;
            }
            if (assignment.UserId != formData.UserId)
            {
                assignment.UserId = formData.UserId;
            }

            this._context.Entry(assignment).State = EntityState.Modified;
            await this._context.SaveChangesAsync();

            return (true, "Assignment updated successfully.");
        }

        public async Task<string> Delete(int id)
        {
            var assignment = this._context.Assignments.FirstOrDefault(x => x.Id == id);
            this._context.Assignments.Remove(assignment);
            await this._context.SaveChangesAsync();

            return "Assignment deleted successfully.";
        }

        public void Complete(int id)
        {
            var assignment = this.GetAssignmentById(id);

            assignment.Status = AssignmentStatus.Completed;
            assignment.CompletedAt = DateTime.Now;
            this._context.SaveChanges();
        }
    }
}
