using Microsoft.AspNetCore.Identity;
using TaskManagmentApp.DAL.Models.Assignments;

namespace TaskManagmentApp.DAL.Models.Account
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal MonthlySalary { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
