using TaskManagmentApp.DAL.ViewModels.Assignment;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.DAL.ViewModels.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.Users = new List<UserViewModel>();
        }

        public List<UserViewModel> Users { get; set; }
        public int UsersCount { get; set; }
        public int AssignmentsCount { get; set; }
        public int AssignmentsInProgressCount { get; set; }
    }
}
