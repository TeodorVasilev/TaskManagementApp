using System.ComponentModel.DataAnnotations;

namespace TaskManagmentApp.DAL.Models.Assignments
{
    public enum AssignmentStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Failed")]
        Failed
    }
}
