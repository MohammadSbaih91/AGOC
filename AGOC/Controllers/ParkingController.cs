using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Controllers
{
    [Authorize]
    public class ParkingController : Controller
    {
        private readonly IParkingManager _parkingManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingController> _logger;

        public ParkingController(IParkingManager parkingManager, IMapper mapper, ILogger<ParkingController> logger)
        {
            _parkingManager = parkingManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching all parking records.");
            var parkings = await _parkingManager.GetAllParkingsAsync();
            var totalParking = parkings.Count();

            var paginatedParking = parkings
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            var mappedModels = _mapper.Map<List<ParkingViewModels>>(paginatedParking);
            var paginatedResult = new Pagination<ParkingViewModels>(mappedModels, totalParking, pageNumber, pageSize);
            ViewData["PaginationAction"] = "Index";
            _logger.LogInformation("Successfully retrieved and mapped parking records.");
            return View(paginatedResult);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching details for parking record with ID {Id}.", id);
            var parking = await _parkingManager.GetParkingByIdAsync(id);
            if (parking == null)
            {
                _logger.LogWarning("Parking record with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<ParkingViewModels>(parking);
            _logger.LogInformation("Successfully retrieved and mapped parking details for ID {Id}.", id);
            return View(mappedModel);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigated to Create Parking view.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkingViewModels parkingViewModel)
        {
            if (parkingViewModel != null)
            {
                _logger.LogInformation("Attempting to create a new parking record.");

                var result = await _parkingManager.AddParkingAsync(parkingViewModel);

                if (result.Success)
                {
                    _logger.LogInformation("Successfully created a new parking record.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError("Error occurred while creating a new parking record: {ErrorMessage}", result.ErrorMessage);
                    ViewData["ErrorMessage"] = result.ErrorMessage;
                    return View(parkingViewModel);
                }
            }

            _logger.LogWarning("Model state is invalid when trying to create a parking record.");
            return View(parkingViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching parking record for editing with ID {Id}.", id);
            var parking = await _parkingManager.GetParkingByIdAsync(id);
            if (parking == null)
            {
                _logger.LogWarning("Parking record with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<ParkingViewModels>(parking);
            _logger.LogInformation("Successfully retrieved and mapped parking record for ID {Id} for editing.", id);
            return View(mappedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ParkingViewModels updatedParkingViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update parking record with ID {Id}.", updatedParkingViewModel.Id);
                    var existingParking = await _parkingManager.GetParkingByIdAsync(updatedParkingViewModel.Id);
                    if (existingParking == null)
                    {
                        _logger.LogWarning("Parking record with ID {Id} not found for update.", updatedParkingViewModel.Id);
                        return NotFound();
                    }

                    _mapper.Map(updatedParkingViewModel, existingParking);
                    await _parkingManager.UpdateParkingAsync(existingParking);
                    _logger.LogInformation("Successfully updated parking record with ID {Id}.", updatedParkingViewModel.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating parking record with ID {Id}.", updatedParkingViewModel.Id);
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            _logger.LogWarning("Model state is invalid when trying to update parking record with ID {Id}.", updatedParkingViewModel.Id);
            return View(updatedParkingViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching parking record for deletion with ID {Id}.", id);
            var parking = await _parkingManager.GetParkingByIdAsync(id);
            if (parking == null)
            {
                _logger.LogWarning("Parking record with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<ParkingViewModels>(parking);
            _logger.LogInformation("Successfully retrieved and mapped parking record for ID {Id} for deletion.", id);
            return View(mappedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Attempting to delete parking record with ID {Id}.", id);
            var existingParking = await _parkingManager.GetParkingByIdAsync(id);
            if (existingParking == null)
            {
                _logger.LogWarning("Parking record with ID {Id} not found for deletion.", id);
                return NotFound();
            }

            var deleteResult = await _parkingManager.DeleteParkingAsync(id);
            if (deleteResult.Success)
            {
                _logger.LogInformation("Successfully deleted parking record with ID {Id}.", id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Error occurred while delete the parking: {ErrorMessage}", deleteResult.ErrorMessage);
                ViewData["ErrorMessage"] = deleteResult.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Search(string searchText = "", int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Searching vehicle parking with filter: {SearchText}", searchText);

            try
            {
                // Search vehicle parking based on searchText
                var parking = await _parkingManager.SearchParking(searchText);

                // Get the total count of filtered parking
                int totalCount = parking.Count();

                // Apply pagination
                var paginatedParking = parking
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagination = new Pagination<ParkingViewModels>(paginatedParking, totalCount, pageNumber, pageSize);
                ViewData["SearchText"] = searchText;
                ViewData["PaginationAction"] = "Search";
                return View("Index", pagination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching vehicle parking.");
                return View("Error");
            }
        }
    }
}