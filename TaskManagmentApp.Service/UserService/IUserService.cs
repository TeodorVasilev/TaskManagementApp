using Microsoft.AspNetCore.Identity;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.Service.UserService
{
    public interface IUserService
    {
        int GetUserId();
        Task<AppUser> GetUserById(int id);
        Task<List<AppUser>> GetAllUsers();
        Task<List<UserViewModel>> LoadUserList();
        Task<EditUserViewModel> LoadEditForm(int id);
        Task<List<string>> LoadRolesList();
        Task<(bool, string)> Create(StoreUserViewModel formData);
        Task<(bool, string)> Update(StoreEditUserViewModel formData);
        Task<(bool, string)> Delete(int id);
    }
}
