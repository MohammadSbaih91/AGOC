using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AGOC.Services;

namespace AGOC.Controllers
{
    [Authorize]
    public class HrController : Controller
    {
        private readonly ILogger<HrController> _logger;
        private readonly HrService _hrService;

        public HrController(ILogger<HrController> logger, HrService hrService)
        {
            _logger = logger;
            _hrService = hrService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeInfo(int empNumber)
        {
            _logger.LogInformation("Attempting to retrieve employee information for empNumber {EmpNumber}.", empNumber);

            try
            {
                var employeeData = await _hrService.GetEmployeeByCodeAsync(empNumber);
                if (employeeData == null)
                {
                    _logger.LogWarning("Employee with empNumber {EmpNumber} not found.", empNumber);
                    return NotFound(new { message = "لا يوجد موظف في الرقم الوظيفي المدخل" }); // Return a 404 response if the employee is not found
                }

                var result = new
                {
                    JobTitle = employeeData.JobTitle,
                    SectionName = employeeData.SectionName,
                    EmployeeName = employeeData.EmployeeName,
                    EmployeeId = employeeData.EmployeeID
                };

                _logger.LogInformation("Successfully retrieved employee information for empNumber {EmpNumber}.", empNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employee information for empNumber {EmpNumber}.", empNumber);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}