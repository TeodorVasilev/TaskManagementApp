namespace TaskManagmentApp.DAL.ViewModels.User
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            this.Roles = new List<string>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
    }
}
