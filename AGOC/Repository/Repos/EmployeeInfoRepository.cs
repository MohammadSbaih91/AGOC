using AGOC.Models;
using AGOC.Repository.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AGOC.Repository.Repos
{
    public class EmployeeInfoRepository : RepositoryBase<EmployeeInfo>, IEmployeeInfoRepository
    {
        private readonly AGOCContext _context;

        public EmployeeInfoRepository(AGOCContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            return await _context.EmployeeInfo.ToListAsync();
        }

        public async Task<EmployeeInfo> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.EmployeeInfo.FindAsync(employeeId);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByDepartmentAsync(string departmentName)
        {
            return await _context.EmployeeInfo
                .Where(emp => emp.DepartmentName == departmentName)
                .ToListAsync();
        }

        public async Task AddEmployeeAsync(EmployeeInfo employee)
        {
            await _context.EmployeeInfo.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(EmployeeInfo employee)
        {
            _context.EmployeeInfo.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.EmployeeInfo.FindAsync(employeeId);
            if (employee != null)
            {
                _context.EmployeeInfo.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
