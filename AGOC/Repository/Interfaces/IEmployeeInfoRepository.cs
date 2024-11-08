﻿using AGOC.Models;

namespace AGOC.Repository.Interfaces
{
    public interface IEmployeeInfoRepository
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo> GetEmployeeByIdAsync(int employeeId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByDepartmentAsync(string departmentName);
        Task AddEmployeeAsync(EmployeeInfo employee);
        Task UpdateEmployeeAsync(EmployeeInfo employee);
        Task DeleteEmployeeAsync(int employeeId);
    }
}
