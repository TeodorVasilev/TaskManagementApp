using TaskManagmentApp.DAL.ViewModels.Home;

namespace TaskManagmentApp.Service.HomeService
{
    public interface IHomeService
    {
        Task<HomeViewModel> LoadHome();
    }
}
