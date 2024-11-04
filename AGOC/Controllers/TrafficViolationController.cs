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
    public class TrafficViolationController : Controller
    {
        private readonly ITrafficViolationManager _trafficViolationManager;
        private readonly ILookupViolationTypeManager _lookupViolationTypeManager;
        private readonly IMapper _mapper;
        private readonly ILogger<TrafficViolationController> _logger;

        public TrafficViolationController(
            ITrafficViolationManager trafficViolationManager,
            IMapper mapper,
            ILookupViolationTypeManager lookupViolationTypeManager,
            ILogger<TrafficViolationController> logger)
        {
            _trafficViolationManager = trafficViolationManager;
            _lookupViolationTypeManager = lookupViolationTypeManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching all traffic violations.");
            var trafficViolations = await _trafficViolationManager.GetAllTrafficViolationsAsync();
            int totalRecords = trafficViolations.Count();
            var paginatedTrafiicViolation = trafficViolations
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            var mappedModels = _mapper.Map<List<TrafficViolationViewModel>>(paginatedTrafiicViolation);
            var paginatedResult = new Pagination<TrafficViolationViewModel>(mappedModels, totalRecords, pageNumber, pageSize);
            ViewData["PaginationAction"] = "Index";
            return View(paginatedResult);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching details for traffic violation with ID {Id}.", id);
            var trafficViolation = await _trafficViolationManager.GetTrafficViolationByIdAsync(id);
            if (trafficViolation == null)
            {
                _logger.LogWarning("Traffic violation with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<TrafficViolationViewModel>(trafficViolation);
            _logger.LogInformation("Successfully retrieved and mapped traffic violation details for ID {Id}.", id);
            return View(mappedModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Navigated to Create Traffic Violation view.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrafficViolationViewModel trafficViolationViewModel)
        {
            if (trafficViolationViewModel != null)
            {
                _logger.LogInformation("Attempting to create a new traffic violation.");

                var trafficViolation = _mapper.Map<TrafficViolation>(trafficViolationViewModel);
                var result = await _trafficViolationManager.AddTrafficViolationAsync(trafficViolation);

                if (result.Success)
                {
                    _logger.LogInformation("Successfully created a new traffic violation.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError("Error occurred while creating a new traffic violation: {ErrorMessage}", result.ErrorMessage);
                    ViewData["ErrorMessage"] = result.ErrorMessage;
                    return View(trafficViolationViewModel);
                }
            }

            _logger.LogWarning("Model is null or invalid when trying to create a traffic violation.");
            return View(trafficViolationViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching traffic violation record for editing with ID {Id}.", id);
            var trafficViolation = await _trafficViolationManager.GetTrafficViolationByIdAsync(id);
            if (trafficViolation == null)
            {
                _logger.LogWarning("Traffic violation with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<TrafficViolationViewModel>(trafficViolation);
            _logger.LogInformation("Successfully retrieved and mapped traffic violation for editing with ID {Id}.", id);
            return View(mappedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrafficViolationViewModel updatedTrafficViolation)
        {
            if (updatedTrafficViolation != null)
            {
                try
                {
                    _logger.LogInformation("Attempting to update traffic violation with ID {Id}.", updatedTrafficViolation.Id);
                    var existingTrafficViolation = await _trafficViolationManager.GetTrafficViolationByIdAsync(updatedTrafficViolation.Id);
                    if (existingTrafficViolation == null)
                    {
                        _logger.LogWarning("Traffic violation with ID {Id} not found for update.", updatedTrafficViolation.Id);
                        return NotFound();
                    }

                    _mapper.Map(updatedTrafficViolation, existingTrafficViolation);
                    await _trafficViolationManager.UpdateTrafficViolationAsync(existingTrafficViolation);
                    _logger.LogInformation("Successfully updated traffic violation with ID {Id}.", updatedTrafficViolation.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating traffic violation with ID {Id}.", updatedTrafficViolation.Id);
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }

            _logger.LogWarning("Model is null or invalid when trying to update traffic violation with ID {Id}.", updatedTrafficViolation?.Id);
            return View(updatedTrafficViolation);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching traffic violation record for deletion with ID {Id}.", id);
            var trafficViolation = await _trafficViolationManager.GetTrafficViolationByIdAsync(id);
            if (trafficViolation == null)
            {
                _logger.LogWarning("Traffic violation with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<TrafficViolationViewModel>(trafficViolation);
            _logger.LogInformation("Successfully retrieved and mapped traffic violation for ID {Id} for deletion.", id);
            return View(mappedModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete traffic violation with ID {Id}.", id);
                await _trafficViolationManager.DeleteTrafficViolationAsync(id);
                _logger.LogInformation("Successfully deleted traffic violation with ID {Id}.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting traffic violation with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public async Task<IActionResult> Search(string searchText = "", int? employeeNumber = null, DateTime? violationDate = null, int? isPaid = null, int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Searching traffic violations with filter: {SearchText}, EmployeeNumber: {EmployeeNumber}, ViolationDate: {ViolationDate}, IsPaid: {IsPaid}",
                searchText, employeeNumber, violationDate, isPaid);

            try
            {
                // Search traffic violations based on filters
                var trafficViolations = await _trafficViolationManager.SearchTrafficViolationsAsync(searchText, employeeNumber, violationDate, isPaid);

                // Get the total count of filtered traffic violations
                int totalCount = trafficViolations.Count();

                // Apply pagination
                var paginatedTrafficViolations = trafficViolations
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Map the paginated traffic violations to ViewModel
                var trafficViolationViewModel = _mapper.Map<List<TrafficViolationViewModel>>(paginatedTrafficViolations);

                // Create Pagination object
                var pagination = new Pagination<TrafficViolationViewModel>(trafficViolationViewModel, totalCount, pageNumber, pageSize);

                // Set the search filters and pagination action in ViewData for the view to handle pagination links
                ViewData["SearchText"] = searchText;
                ViewData["EmployeeNumber"] = employeeNumber;
                ViewData["ViolationDate"] = violationDate;
                ViewData["IsPaid"] = isPaid;
                ViewData["PaginationAction"] = "Search"; // Ensure pagination links call the Search action

                // Return the Index view with the filtered and paginated results
                return View("Index", pagination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching traffic violations.");
                return View("Error");
            }
        }

        public async Task<IActionResult> PaidViolations(int id)
        {
            var violation = await _trafficViolationManager.GetTrafficViolationByIdAsync(id);

            if (violation == null)
            {
                return NotFound();
            }
            await _trafficViolationManager.PayTrafficViolationAsync(violation);

            return RedirectToAction("Index");
        }
    }
}