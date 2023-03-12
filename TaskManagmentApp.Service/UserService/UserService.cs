using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagmentApp.DAL.Data;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;


        public UserService(AppDbContext context, IHttpContextAccessor httpContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
            this._httpContext = httpContext;
        }

        public int GetUserId()
        {
            return int.Parse(this._httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await this._context.Users.ToListAsync();
        }

        public async Task<EditUserViewModel> LoadEditForm(int id)
        {
            var user = await this.GetUserById(id);
            var roles = await this._context.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                MonthlySalary = user.MonthlySalary,
                Role = userRoles.First(),
                Roles = roles,
            };

            return model;
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await this._context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(bool, string)> Create(StoreUserViewModel formData)
        {
            if (string.IsNullOrWhiteSpace(formData.Name) || formData.Name.Length < 3)
            {
                return (false, "Name must be at least 3 characters long.");
            }
            if (string.IsNullOrWhiteSpace(formData.PhoneNumber) || formData.PhoneNumber.Length < 10)
            {
                return (false, "Phone number must be at least 10 characters long.");
            }
            if (DateTime.Now.Year - formData.DateOfBirth.Year < 18)
            {
                return (false, "User must be at least 18 years old");
            }

            var user = new AppUser
            {
                Name = formData.Name,
                Email = formData.Email,
                PhoneNumber = formData.PhoneNumber,
                DateOfBirth = formData.DateOfBirth,
                MonthlySalary = formData.MonthlySalary,
                UserName = formData.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var password = Guid.NewGuid().ToString().Substring(0, 8);
            password += "?Aa1";

            var result = await _userManager.CreateAsync(user, password);
            string message = "";

            if (result.Succeeded)
            {
                message = $"The password for user {user.UserName} is: {password} Keep the password as it cannot be recovered.";
                await this._userManager.AddToRoleAsync(user, formData.Role);
            }

            return (result.Succeeded, message);
        }

        public async Task<(bool, string)> Delete(int id)
        {
            if(id == 1)
            {
                return (false, "Cannot delete the admin user");
            }

            var user = this._context.Users.Where(u => u.Id == id).FirstOrDefault();

            var message = "Successfully deleted user!";
            var result = await _userManager.DeleteAsync(user);

            return (result.Succeeded, message);
        }

        public async Task<(bool, string)> Update(StoreEditUserViewModel formData)
        {
            if (string.IsNullOrWhiteSpace(formData.Name) || formData.Name.Length < 3)
            {
                return (false, "Name must be at least 3 characters long.");
            }
            if (string.IsNullOrWhiteSpace(formData.PhoneNumber) || formData.PhoneNumber.Length < 10)
            {
                return (false, "Phone number must be at least 10 characters long.");
            }
            if (DateTime.Now.Year - formData.DateOfBirth.Year < 18)
            {
                return (false, "User must be at least 18 years old");
            }

            var user = await this.GetUserById(formData.Id);
            var roles = await this._userManager.GetRolesAsync(user);
            var role = roles.First();

            if (user.Name != formData.Name)
            {
                user.Name = formData.Name;
            }
            if (user.Email != formData.Email)
            {
                user.Email = formData.Email;
            }
            if (user.PhoneNumber != formData.Email)
            {
                user.PhoneNumber = formData.PhoneNumber;
            }
            if (user.DateOfBirth != formData.DateOfBirth)
            {
                user.DateOfBirth = formData.DateOfBirth;
            }
            if (this._httpContext.HttpContext.User.IsInRole("Admin"))
            {
                if (user.MonthlySalary != formData.MonthlySalary)
                {
                    user.MonthlySalary = formData.MonthlySalary;
                }
                if (role != formData.Role)
                {
                    var removeFromRole = await this._userManager.RemoveFromRoleAsync(user, role);
                    if (removeFromRole.Succeeded)
                    {
                        await this._userManager.AddToRoleAsync(user, formData.Role);
                    }
                }
            }

            var result = await this._userManager.UpdateAsync(user);
            string message = "";

            if (result.Succeeded)
            {
                message = $"Successfully updated data for user: {user.UserName}";
            }

            return (result.Succeeded, message);
        }

        public async Task<List<UserViewModel>> LoadUserList()
        {
            var users = await this.GetAllUsers();

            var list = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    MonthlySalary = user.MonthlySalary,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };

                list.Add(userViewModel);
            }

            return list;
        }

        public async Task<List<string>> LoadRolesList()
        {
            var roles = await _context.Roles.Select(r => r.Name).ToListAsync();
            return roles;
        }
    }
}
