using Microsoft.EntityFrameworkCore;
using TaskManagmentApp.DAL.Data;
using TaskManagmentApp.DAL.Models.Assignments;
using TaskManagmentApp.DAL.ViewModels.Home;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.Service.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly AppDbContext _context;
        public HomeService(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<HomeViewModel> LoadHome()
        {
            var model = new HomeViewModel();

            model.Users = await this.GetTopUsers();
            model.UsersCount = await this.GetUsersCount();
            model.AssignmentsCount = await this.GetAssignmentsCount();
            model.AssignmentsInProgressCount = await this.GetInProgressAssignmentsCount();

            return model;
        }

        public async Task<List<UserViewModel>> GetTopUsers()
        {
            var users = await this._context.Users.Include(u => u.Assignments).ToListAsync();

            var lastMonth = DateTime.Now.AddMonths(-1);
            var topUsers = users
                .Where(u => u.Assignments.Any(a => a.Status == AssignmentStatus.Completed && a.CompletedAt >= lastMonth))
                .OrderByDescending(u => u.Assignments.Count(a => a.Status == AssignmentStatus.Completed && a.CompletedAt >= lastMonth))
                .Take(5)
                .ToList();

            var list = topUsers.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                MonthlySalary = u.MonthlySalary,
                CompletedAssignments = u.Assignments.Count(a => a.Status == AssignmentStatus.Completed),
            }).ToList();

            return list;
        }

        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetAssignmentsCount()
        {
            return await _context.Assignments.CountAsync();
        }

        public async Task<int> GetInProgressAssignmentsCount()
        {
            return await _context.Assignments.Where(a => a.Status == AssignmentStatus.InProgress).CountAsync();
        }
    }
}
