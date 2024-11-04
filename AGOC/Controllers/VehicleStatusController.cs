using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.Controllers
{
    [Authorize]
    public class VehicleStatusController : Controller
    {
        private readonly IVehicleStatusManager _vehicleStatusManager;
        private readonly IMapper _mapper;

        public VehicleStatusController(IVehicleStatusManager vehicleStatusManager, IMapper mapper)
        {
            _vehicleStatusManager = vehicleStatusManager;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> Index()
        {
            var vehicleStatuses = await _vehicleStatusManager.GetAllVehicleStatusesAsync();
            var mappedModels = _mapper.Map<IEnumerable<VehicleStatusViewModel>>(vehicleStatuses);
            return View(mappedModels);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var vehicleStatus = await _vehicleStatusManager.GetVehicleStatusByIdAsync(id);
            if (vehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<VehicleStatusViewModel>(vehicleStatus);
            return View(mappedModel);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        /// <summary>
        /// POST: /VehicleStatus/Create
        /// </summary>
        /// <param name="vehicleStatusViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleStatusViewModel vehicleStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                var vehicleStatus = _mapper.Map<VehicleStatus>(vehicleStatusViewModel);
                await _vehicleStatusManager.AddVehicleStatusAsync(vehicleStatus);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleStatusViewModel);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleStatus = await _vehicleStatusManager.GetVehicleStatusByIdAsync(id);
            if (vehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<VehicleStatusViewModel>(vehicleStatus);
            return View(mappedModel);
        }

        
        /// <summary>
        /// POST: /VehicleStatus/Edit/{id}
        /// </summary>
        /// <param name="updatedVehicleStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleStatusViewModel updatedVehicleStatus)
        {
            if (ModelState.IsValid)
            {
                var existingVehicleStatus = await _vehicleStatusManager.GetVehicleStatusByIdAsync(updatedVehicleStatus.Id);
                if (existingVehicleStatus == null)
                {
                    return NotFound();
                }

                _mapper.Map(updatedVehicleStatus, existingVehicleStatus);
                await _vehicleStatusManager.UpdateVehicleStatusAsync(existingVehicleStatus);
                return RedirectToAction(nameof(Index));
            }
            return View(updatedVehicleStatus);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleStatus = await _vehicleStatusManager.GetVehicleStatusByIdAsync(id);
            if (vehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<VehicleStatusViewModel>(vehicleStatus);
            return View(mappedModel);
        }

        
        /// <summary>
        /// POST: /VehicleStatus/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleStatus = await _vehicleStatusManager.GetVehicleStatusByIdAsync(id);
            if (vehicleStatus == null)
            {
                return NotFound();
            }

            await _vehicleStatusManager.DeleteVehicleStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}