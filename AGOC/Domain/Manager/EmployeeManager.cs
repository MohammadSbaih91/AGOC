using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Services
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            return await _unitOfWork.EmployeeInfoRepository.GetAllEmployeesAsync();
        }

        public async Task<EmployeeInfo> GetEmployeeByIdAsync(int employeeId)
        {
            return await _unitOfWork.EmployeeInfoRepository.GetEmployeeByIdAsync(employeeId);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByDepartmentAsync(string departmentName)
        {
            return await _unitOfWork.EmployeeInfoRepository.GetEmployeesByDepartmentAsync(departmentName);
        }
    }
}
