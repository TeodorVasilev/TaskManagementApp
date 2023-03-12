using TaskManagmentApp.DAL.Models.Account;

namespace TaskManagmentApp.DAL.Models.Assignments
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CompletedAt { get; set; }
        public AssignmentStatus Status { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
