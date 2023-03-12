using TaskManagmentApp.DAL.Models.Assignments;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.DAL.ViewModels.Assignment
{
    public class EditAssignmentViewModel
    {
        public EditAssignmentViewModel()
        {
            this.Users = new List<UserViewModel>();
            this.StatusOptions = Enum.GetValues(typeof(AssignmentStatus)).Cast<AssignmentStatus>().ToList();
        }

        public AssignmentViewModel Assignment { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<AssignmentStatus> StatusOptions { get; set; }
    }
}
