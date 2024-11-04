using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IEmployeeManager
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo> GetEmployeeByIdAsync(int employeeId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByDepartmentAsync(string departmentName);
    }
}
