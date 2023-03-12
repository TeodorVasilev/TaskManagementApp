using TaskManagmentApp.DAL.Models.Assignments;
using TaskManagmentApp.DAL.ViewModels.Assignment;
using TaskManagmentApp.DAL.ViewModels.User;

namespace TaskManagmentApp.Service.AssignmentService
{
    public interface IAssignmentService
    {
        Task<List<UserViewModel>> LoadCreateForm();
        Task<EditAssignmentViewModel> LoadEditForm(int id);
        void Complete(int id);
        Task<(bool, string)> Update(StoreAssignmentViewModel formData);
        Task<(bool, string)> Create(StoreAssignmentViewModel formData);
        Task<string> Delete(int id);
        Task<List<Assignment>> GetAllAssignments();
        Assignment GetAssignmentById(int id);
        Task<List<AssignmentViewModel>> LoadAssignmentList();
    }
}
