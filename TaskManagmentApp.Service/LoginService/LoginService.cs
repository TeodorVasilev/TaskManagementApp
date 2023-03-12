using Microsoft.AspNetCore.Identity;
using TaskManagmentApp.DAL.Models.Account;
using TaskManagmentApp.DAL.ViewModels.Account;

namespace TaskManagmentApp.Service.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginService(SignInManager<AppUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAttempt(LoginViewModel formData)
        {
            return await this._signInManager.PasswordSignInAsync(formData.Email, formData.Password, false, false);
        }

        public async void Logout()
        {
            await this._signInManager.SignOutAsync();
        }
    }
}
