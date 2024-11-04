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
    public class VehiclesController : Controller
    {
        private readonly IVehicleManager _vehicleManager;
        private readonly IMapper _iMapper;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleManager vehicleManager, IMapper iMapper, ILogger<VehiclesController> logger)
        {
            _iMapper = iMapper;
            _vehicleManager = vehicleManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Get the total count of vehicles
                var totalCount = await _vehicleManager.GetTotalCountAsync();
                var vehicles = await _vehicleManager.GetPagedVehiclesAsync(pageNumber, pageSize);
                // Map the data to the ViewModel
                var mappedVehicles = _iMapper.Map<List<VehicleViewModel>>(vehicles);

                var pagination = new Pagination<VehicleViewModel>(mappedVehicles, totalCount, pageNumber, pageSize);
                ViewData["SearchText"] = "";
                ViewData["VehicleTypeId"] = 0;
                ViewData["PaginationAction"] = "Index";

                _logger.LogInformation("Successfully retrieved paginated vehicles.");
                return View(pagination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated vehicles.");
                return View("Error");
            }
        }

        public async Task<IActionResult> DetailsByPlate(string LicensePlateNumber)
        {
            _logger.LogInformation("Fetching vehicle details by license plate number: {LicensePlateNumber}", LicensePlateNumber);
            var vehicle = await _vehicleManager.GetVehicleByLicensePlateAsync(LicensePlateNumber);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with license plate number {LicensePlateNumber} not found.", LicensePlateNumber);
                return NotFound(new { message = "لا يوجد مركبة بالرقم اللوحة المدخلة" });
            }

            var mappedVehicle = _iMapper.Map<VehicleViewModel>(vehicle);
            _logger.LogInformation("Successfully retrieved and mapped vehicle details for license plate number: {LicensePlateNumber}", LicensePlateNumber);
            return Json(mappedVehicle);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching vehicle details by ID: {Id}", id);
            var vehicle = await _vehicleManager.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedVehicle = _iMapper.Map<VehicleViewModel>(vehicle);
            _logger.LogInformation("Successfully retrieved and mapped vehicle details for ID: {Id}", id);
            return View(mappedVehicle);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigated to Create Vehicle view.");
            var vehicleViewModel = new VehicleViewModel
            {
                VehicleTypeId = 1
            }; return View(vehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to create a new vehicle.");
                    var newVehicle = _iMapper.Map<Vehicle>(vehicleViewModel);
                    await _vehicleManager.AddVehicleAsync(newVehicle);
                    _logger.LogInformation("Successfully created a new vehicle.");
                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Error occurred while creating a new vehicle.");
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for vehicle creation.");
            }

            return View(vehicleViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching vehicle details for editing with ID: {Id}", id);
            var vehicle = await _vehicleManager.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedVehicle = _iMapper.Map<VehicleViewModel>(vehicle);
            _logger.LogInformation("Successfully retrieved and mapped vehicle for editing with ID: {Id}", id);
            return View(mappedVehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel updatedVehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update vehicle with ID: {Id}.", updatedVehicle.Id);
                    var existingVehicle = await _vehicleManager.GetVehicleByIdAsync(updatedVehicle.Id);
                    if (existingVehicle == null)
                    {
                        _logger.LogWarning("Vehicle with ID {Id} not found for update.", updatedVehicle.Id);
                        return NotFound();
                    }

                    _iMapper.Map(updatedVehicle, existingVehicle);
                    await _vehicleManager.Update(existingVehicle);
                    _logger.LogInformation("Successfully updated vehicle with ID: {Id}.", updatedVehicle.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Error occurred while updating vehicle with ID: {Id}.", updatedVehicle.Id);
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for vehicle update with ID: {Id}.", updatedVehicle.Id);
            }

            return View(updatedVehicle);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete vehicle with ID: {Id}.", id);
                await _vehicleManager.DeleteVehicle(id);
                _logger.LogInformation("Successfully deleted vehicle with ID: {Id}.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting vehicle with ID: {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckHandoverAndViolations(int id)
        {
            _logger.LogInformation("Checking if vehicle with ID {Id} is handed over or has violations.", id);

            try
            {
                bool isHandovered = await _vehicleManager.CheckHandoveredVehicles(id);
                if (isHandovered)
                {
                    _logger.LogWarning("Vehicle with ID {Id} is handed over and cannot be deleted.", id);
                    return Json(new { success = false, errorMessage = "لا يمكن حذف مركبة مخصصة" });
                }

                bool hasViolations = await _vehicleManager.HasTrafficViolations(id);
                if (hasViolations)
                {
                    _logger.LogWarning("Vehicle with ID {Id} has associated violations.", id);
                    return Json(new { success = false, confirmMessage = "هذه المركبة لديها مخالفات مرتبطة. سيتم حذف المخالفات مع المركبة. هل تريد المتابعة؟" });
                }

                await _vehicleManager.DeleteVehicle(id);
                _logger.LogInformation("Successfully deleted vehicle with ID {Id} after checks.", id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking handover and violations for vehicle with ID {Id}.", id);
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        public async Task<IActionResult> Search(string searchText = "", int vehicleTypeId = 0, int pageNumber = 1, int pageSize = 10)
        {
            var vehicles = await _vehicleManager.SearchVehiclesAsync(searchText, vehicleTypeId);
            int totalCount = vehicles.Count();
            var paginatedVehicles = vehicles.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var mappedVehicles = _iMapper.Map<List<VehicleViewModel>>(paginatedVehicles);
            var pagination = new Pagination<VehicleViewModel>(mappedVehicles, totalCount, pageNumber, pageSize);

            ViewData["SearchText"] = searchText;
            ViewData["VehicleTypeId"] = vehicleTypeId;
            ViewData["PaginationAction"] = "Search"; // Make sure pagination uses the "Search" action

            return View("Index", pagination); // Return the Index view with the search results
        }
    }
}