using Microsoft.AspNetCore.Identity;
using TaskManagmentApp.DAL.ViewModels.Account;

namespace TaskManagmentApp.Service.LoginService
{
    public interface ILoginService
    {
        Task<SignInResult> LoginAttempt(LoginViewModel formData);
        void Logout();
    }
}
