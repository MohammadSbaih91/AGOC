using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Controllers
{
    [Authorize]
    public class HandoverController : Controller
    {
        private readonly IMapper _iMapper;
        private readonly IVehicleHandoverManager _vehicleHandoverManager;
        private readonly HrService _hrService;
        private readonly ILogger<HandoverController> _logger;

        public HandoverController(IVehicleHandoverManager vehicleHandoverManager, IMapper iMapper, HrService hrService, ILogger<HandoverController> logger)
        {
            _iMapper = iMapper;
            _vehicleHandoverManager = vehicleHandoverManager;
            _hrService = hrService;
            _logger = logger;
        }

        #region Handover

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching all vehicle handovers.");

            try
            {
                // Fetch all handovers
                var handovers = await _vehicleHandoverManager.GetAllVehicleHandoversAsync();

                // Total count for pagination
                int totalRecords = handovers.Count();

                // Paginated data
                var paginatedHandovers = handovers
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var handoversViewModel = _iMapper.Map<List<VehicleHandoverViewModel>>(paginatedHandovers);
                var paginatedResult = new Pagination<VehicleHandoverViewModel>(handoversViewModel, totalRecords, pageNumber, pageSize);
                ViewData["PaginationAction"] = "Index";
                return View(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all vehicle handovers.");
                ViewData["ErrorMessage"] = "An error occurred while loading the handover list.";
                return View(new Pagination<VehicleHandoverViewModel>());
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigating to the Create Handover page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleHandoverViewModel handoverViewModel)
        {
            _logger.LogInformation("Attempting to create a new vehicle handover for employee {EmployeeCode}.", handoverViewModel.EmployeeCode);

            var handover = _iMapper.Map<VehicleHandover>(handoverViewModel);
            var result = await _vehicleHandoverManager.AddVehicleHandoverAsync(handover);

            if (result.Success)
            {
                _logger.LogInformation("Successfully created a new vehicle handover for employee {EmployeeCode}.", handoverViewModel.EmployeeCode);
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError("An error occurred while creating a new vehicle handover for employee {EmployeeCode}: {ErrorMessage}", handoverViewModel.EmployeeCode, result.ErrorMessage);
                ViewData["ErrorMessage"] = result.ErrorMessage;
                return View(handoverViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            _logger.LogInformation("Attempting to cancel vehicle handover with ID {HandoverId}.", id);

            var result = await _vehicleHandoverManager.CancelVehicleHandoverAsync(id);

            if (result.Success)
            {
                _logger.LogInformation("Successfully canceled vehicle handover with ID {HandoverId}.", id);
            }
            else
            {
                _logger.LogError("An error occurred while canceling vehicle handover with ID {HandoverId}: {ErrorMessage}", id, result.ErrorMessage);
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred while canceling the handover.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int handoverId)
        {
            _logger.LogInformation("Fetching details for vehicle handover with ID {HandoverId}.", handoverId);
            try
            {
                var handover = await _vehicleHandoverManager.GetVehicleHandoverByIdAsync(handoverId);
                if (handover == null)
                {
                    _logger.LogWarning("Vehicle handover with ID {HandoverId} not found.", handoverId);
                    return NotFound();
                }

                var handoverViewModel = _iMapper.Map<VehicleHandoverViewModel>(handover);
                return View(handoverViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching details for vehicle handover with ID {HandoverId}.", handoverId);
                ViewData["ErrorMessage"] = "An error occurred while loading the handover details.";
                return View(new VehicleHandoverViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int handoverId)
        {
            _logger.LogInformation("Attempting to delete vehicle handover with ID {HandoverId}.", handoverId);
            try
            {
                var existingHandover = await _vehicleHandoverManager.GetVehicleHandoverByIdAsync(handoverId);
                if (existingHandover == null)
                {
                    _logger.LogWarning("Vehicle handover with ID {HandoverId} not found.", handoverId);
                    return NotFound();
                }

                await _vehicleHandoverManager.DeleteVehicleHandoverAsync(handoverId);
                _logger.LogInformation("Successfully deleted vehicle handover with ID {HandoverId}.", handoverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting vehicle handover with ID {HandoverId}.", handoverId);
                ViewData["ErrorMessage"] = "An error occurred while deleting the handover.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetEmployeeInfo(int empNumber)
        {
            _logger.LogInformation("Fetching employee info for employee number {EmployeeNumber}.", empNumber);
            try
            {
                var empInfo = await _hrService.GetEmployeeByCodeAsync(empNumber);
                if (empInfo == null)
                {
                    _logger.LogWarning("Employee with number {EmployeeNumber} not found.", empNumber);
                    return NotFound(); // Return a 404 response if the employee is not found
                }

                var result = new
                {
                    JobTitle = empInfo.JobTitle,
                    SectionName = empInfo.SectionName,
                    EmployeeName = empInfo.EmployeeName,
                    EmployeeId = empInfo.EmployeeID
                };

                _logger.LogInformation("Successfully fetched employee info for employee number {EmployeeNumber}.", empNumber);
                return Ok(result); // Return the employee info as JSON if found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee info for employee number {EmployeeNumber}.", empNumber);
                return StatusCode(500, "An error occurred while fetching employee information.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(List<int> selectedHandoverIds)
        {
            if (selectedHandoverIds == null || !selectedHandoverIds.Any())
            {
                ModelState.AddModelError(string.Empty, "No handovers selected for approval.");
                return RedirectToAction("Index");
            }

            await _vehicleHandoverManager.ApproveHandoversAsync(selectedHandoverIds);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string searchText = "", int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Searching vehicle handovers with filter: {SearchText}", searchText);

            try
            {
                // Search vehicle handovers based on searchText
                var handovers = await _vehicleHandoverManager.SearchHandoversAsync(searchText);

                // Get the total count of filtered handovers
                int totalCount = handovers.Count();

                // Apply pagination
                var paginatedHandovers = handovers
                    .OrderByDescending(x=> x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Map the paginated handovers to ViewModel
                var handoversViewModel = _iMapper.Map<List<VehicleHandoverViewModel>>(paginatedHandovers);

                // Create Pagination object
                var pagination = new Pagination<VehicleHandoverViewModel>(handoversViewModel, totalCount, pageNumber, pageSize);

                // Set the search text and pagination action in ViewData for the view to handle pagination links
                ViewData["SearchText"] = searchText;
                ViewData["PaginationAction"] = "Search"; // Ensure pagination links call the Search action

                // Return the Index view with the filtered and paginated results
                return View("Index", pagination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching vehicle handovers.");
                return View("Error");
            }
        }

        #endregion Handover
    }
}