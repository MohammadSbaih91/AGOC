using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Web.Controllers
{
    public class VehiclesLookupMainController : Controller
    {
        private readonly IVehiclesLookupMainManager _lookupMainManager;
        private readonly IMapper _mapper;

        public VehiclesLookupMainController(IVehiclesLookupMainManager lookupMainManager, IMapper mapper)
        {
            _lookupMainManager = lookupMainManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Fetch all vehicle lookup main entries
                var vehicleLookupMains = await _lookupMainManager.GetAllVehiclesLookupMainesAsync();
                int totalCount = vehicleLookupMains.Count();

                // Paginate the entries
                var paginatedVehicles = vehicleLookupMains
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pagination = new Pagination<VehiclesLookupMain>(paginatedVehicles, totalCount, pageNumber, pageSize);

                // Create the view model
                var viewModel = new VehiclesLookupMainIndexViewModel
                {
                    PaginatedVehicles = pagination,
                    CreateVehicleModel = new VehiclesLookupMain() // For the create form
                };

                // You can use AutoMapper here if you need to map specific data, e.g., form data
                var mappedViewModel = _mapper.Map<VehiclesLookupMainIndexViewModel>(viewModel);

                ViewData["PaginationAction"] = "Index";
                return View(mappedViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehiclesLookupMainIndexViewModel model)
        {
            var lookupMain = new VehiclesLookupMain
            {
                Description = model.CreateVehicleModel.Description,
            };
            
                await _lookupMainManager.AddVehiclesLookupMainAsync(lookupMain);
                return RedirectToAction("Index");
            
           
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _lookupMainManager.GetVehiclesLookupMainByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehiclesLookupMain model)
        {
            if (ModelState.IsValid)
            {
                await _lookupMainManager.UpdateVehiclesLookupMainAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _lookupMainManager.DeleteVehiclesLookupMainAsync(id);
            return RedirectToAction("Index");
        }
    }
}