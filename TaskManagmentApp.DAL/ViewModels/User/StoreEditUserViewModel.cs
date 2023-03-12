namespace TaskManagmentApp.DAL.ViewModels.User
{
    public class StoreEditUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Role { get; set; }
    }
}
