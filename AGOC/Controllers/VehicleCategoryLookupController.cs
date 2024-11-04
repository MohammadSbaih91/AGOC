using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Web.Controllers
{
    [Authorize]
    public class VehicleCategoryLookupController : Controller
    {
        private readonly IVehicleCategoryLookupManager _categoryLookupManager;
        private readonly IVehiclesLookupDetaileManager _vehiclesLookupDetaileManager;

        public VehicleCategoryLookupController(IVehicleCategoryLookupManager categoryLookupManager, IVehiclesLookupDetaileManager vehiclesLookupDetaileManager)
        {
            _categoryLookupManager = categoryLookupManager;
            _vehiclesLookupDetaileManager = vehiclesLookupDetaileManager;
        }

        // Index with Pagination
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var vehicleCategoryLookups = await _categoryLookupManager.GetAllAsync();
            int totalCount = vehicleCategoryLookups.Count();

            var paginatedCategories = vehicleCategoryLookups
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagination = new Pagination<VehicleCategoryLookup>(paginatedCategories, totalCount, pageNumber, pageSize);

            // Populate the ViewBag for the dropdown in the modal form
            ViewBag.VehiclesLookupDetails = _vehiclesLookupDetaileManager.GetAllVehiclesLookupDetaileesAsync().Result;

            var viewModel = new VehicleCategoryLookupIndexViewModel
            {
                PaginatedCategories = pagination,
                CreateCategoryModel = new VehicleCategoryLookup()
            };
            ViewData["PaginationAction"] = "Index";
            return View(viewModel);
        }

        public IActionResult Create()
        {
            // Assuming you have a method to fetch the details from your manager or service
            var vehiclesLookupDetails = _vehiclesLookupDetaileManager.GetAllVehiclesLookupDetaileesAsync().Result;

            ViewBag.VehiclesLookupDetails = vehiclesLookupDetails;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleCategoryLookupIndexViewModel model)
        {
            if (model != null)
            {
                var categoryLookup = new VehicleCategoryLookup
                {
                    Description = model.CreateCategoryModel.Description,
                    VehiclesLookupDetailId = model.CreateCategoryModel.VehiclesLookupDetailId,
                    CreatedOn = DateTime.UtcNow,
                };
                await _categoryLookupManager.AddAsync(categoryLookup);
                return RedirectToAction("Index");
            }

            // Reload the ViewBag in case of validation errors
            var vehiclesLookupDetails = _vehiclesLookupDetaileManager.GetAllVehiclesLookupDetaileesAsync().Result;
            ViewBag.VehiclesLookupDetails = vehiclesLookupDetails;

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Get the category by its ID
            var model = await _categoryLookupManager.GetByIdAsync(id);

            // Ensure that the dropdown list is populated
            var vehiclesLookupDetails = await _vehiclesLookupDetaileManager.GetAllVehiclesLookupDetaileesAsync();
            ViewBag.VehiclesLookupDetails = vehiclesLookupDetails;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleCategoryLookup model)
        {
            if (ModelState.IsValid)
            {
                await _categoryLookupManager.UpdateAsync(model);
                return RedirectToAction("Index");
            }

            // Reload the ViewBag in case of validation errors
            var vehiclesLookupDetails = await _vehiclesLookupDetaileManager.GetAllVehiclesLookupDetaileesAsync();
            ViewBag.VehiclesLookupDetails = vehiclesLookupDetails;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Perform the deletion
                await _categoryLookupManager.DeleteAsync(id);

                // Return a JSON response indicating success
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a JSON error message
                return Json(new { success = false, errorMessage = "An error occurred while deleting the category: " + ex.Message });
            }
        }
    }
}