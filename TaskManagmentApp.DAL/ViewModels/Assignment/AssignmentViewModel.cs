using TaskManagmentApp.DAL.Models.Assignments;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.DAL.ViewModels.Assignment
{
    public class AssignmentViewModel
    {
        public AssignmentViewModel()
        {
            Status = new AssignmentStatus();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; }
        public int UserId{ get; set; }
    }
}
