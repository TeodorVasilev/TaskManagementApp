using TaskManagmentApp.DAL.Models.Assignments;

namespace TaskManagmentApp.DAL.ViewModels.Assignment
{
    public class StoreAssignmentViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; }
        public int UserId { get; set; }
    }
}
