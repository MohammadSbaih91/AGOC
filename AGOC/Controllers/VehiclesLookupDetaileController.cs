using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Web.Controllers
{
    public class VehiclesLookupDetaileController : Controller
    {
        private readonly IVehiclesLookupDetaileManager _lookupDetaileManager;

        public VehiclesLookupDetaileController(IVehiclesLookupDetaileManager lookupDetaileManager)
        {
            _lookupDetaileManager = lookupDetaileManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Fetch all vehicle lookup details
                var vehicleLookupDetails = await _lookupDetaileManager.GetAllVehiclesLookupDetaileesAsync();
                int totalCount = vehicleLookupDetails.Count();

                // Paginate the entries
                var paginatedDetails = vehicleLookupDetails.
                   OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagination = new Pagination<VehiclesLookupDetaile>(paginatedDetails, totalCount, pageNumber, pageSize);

                // Create the ViewModel
                var viewModel = new VehiclesLookupDetaileIndexViewModel
                {
                    PaginatedVehiclesDetails = pagination,
                    CreateVehicleDetailModel = new VehiclesLookupDetaile() // For the create form
                };
                ViewData["PaginationAction"] = "Index";

                // Fetch all VehiclesLookupMain entries and pass them to the view
                var vehiclesLookupMains = await _lookupDetaileManager.GetAllVehiclesLookupMainesAsync();
                ViewBag.VehiclesLookupMains = vehiclesLookupMains;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehiclesLookupDetaileIndexViewModel model)
        {
            if (model != null)
            {
                var detailLookup = new VehiclesLookupDetaile
                {
                    VehiclesLookupMainId= model.CreateVehicleDetailModel.VehiclesLookupMainId,
                    Description = model.CreateVehicleDetailModel.Description
                };
                await _lookupDetaileManager.AddVehiclesLookupDetaileAsync(detailLookup);
                return RedirectToAction("Index");
            }

            var vehiclesLookupMains = await _lookupDetaileManager.GetAllVehiclesLookupMainesAsync();
            ViewBag.VehiclesLookupMains = vehiclesLookupMains;

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehiclesLookupDetaile = await _lookupDetaileManager.GetVehiclesLookupDetaileByIdAsync(id);
            if (vehiclesLookupDetaile == null)
            {
                return NotFound();
            }

            // Populate ViewBag for the dropdown
            ViewBag.VehiclesLookupMains = await _lookupDetaileManager.GetAllVehiclesLookupMainesAsync();

            return View(vehiclesLookupDetaile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehiclesLookupDetaile model)
        {
            if (ModelState.IsValid)
            {
                await _lookupDetaileManager.UpdateVehiclesLookupDetaileAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _lookupDetaileManager.DeleteVehiclesLookupDetaileAsync(id);
            return RedirectToAction("Index");
        }
    }
}