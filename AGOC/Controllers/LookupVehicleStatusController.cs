using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.Controllers
{
    [Authorize]
    public class LookupVehicleStatusController : Controller
    {
        private readonly ILookupVehicleStatusManager _lookupVehicleStatusManager;
        private readonly IMapper _mapper;

        public LookupVehicleStatusController(ILookupVehicleStatusManager lookupVehicleStatusManager, IMapper mapper)
        {
            _lookupVehicleStatusManager = lookupVehicleStatusManager;
            _mapper = mapper;
        }

        /// <summary>
        /// GET: /LookupVehicleStatus
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var lookupVehicleStatuses = await _lookupVehicleStatusManager.GetAllLookupVehicleStatusesAsync();
            var mappedModels = _mapper.Map<IEnumerable<LookupVehicleStatusViewModel>>(lookupVehicleStatuses);
            return View(mappedModels);
        }

        /// <summary>
        /// GET: /LookupVehicleStatus/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id)
        {
            var lookupVehicleStatus = await _lookupVehicleStatusManager.GetLookupVehicleStatusByIdAsync(id);
            if (lookupVehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupVehicleStatusViewModel>(lookupVehicleStatus);
            return View(mappedModel);
        }

        /// <summary>
        /// GET: /LookupVehicleStatus/Create
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /LookupVehicleStatus/Create
        /// </summary>
        /// <param name="lookupVehicleStatusViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LookupVehicleStatusViewModel lookupVehicleStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = _mapper.Map<LookupVehicleStatus>(lookupVehicleStatusViewModel);
                await _lookupVehicleStatusManager.AddLookupVehicleStatusAsync(mappedModel);
                return RedirectToAction(nameof(Index));
            }
            return View(lookupVehicleStatusViewModel);
        }

        /// <summary>
        /// GET: /LookupVehicleStatus/Edit/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var lookupVehicleStatus = await _lookupVehicleStatusManager.GetLookupVehicleStatusByIdAsync(id);
            if (lookupVehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupVehicleStatusViewModel>(lookupVehicleStatus);
            return View(mappedModel);
        }

        /// <summary>
        /// POST: /LookupVehicleStatus/Edit/{id}
        /// </summary>
        /// <param name="updatedLookupVehicleStatus"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LookupVehicleStatusViewModel updatedLookupVehicleStatus)
        {
            if (ModelState.IsValid)
            {
                var existingLookupVehicleStatus = await _lookupVehicleStatusManager.GetLookupVehicleStatusByIdAsync(updatedLookupVehicleStatus.Id);
                if (existingLookupVehicleStatus == null)
                {
                    return NotFound();
                }

                _mapper.Map(updatedLookupVehicleStatus, existingLookupVehicleStatus);
                await _lookupVehicleStatusManager.UpdateLookupVehicleStatusAsync(existingLookupVehicleStatus);
                return RedirectToAction(nameof(Index));
            }
            return View(updatedLookupVehicleStatus);
        }

        /// <summary>
        /// GET: /LookupVehicleStatus/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            var lookupVehicleStatus = await _lookupVehicleStatusManager.GetLookupVehicleStatusByIdAsync(id);
            if (lookupVehicleStatus == null)
            {
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupVehicleStatusViewModel>(lookupVehicleStatus);
            return View(mappedModel);
        }

        /// <summary>
        /// POST: /LookupVehicleStatus/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingLookupVehicleStatus = await _lookupVehicleStatusManager.GetLookupVehicleStatusByIdAsync(id);
            if (existingLookupVehicleStatus == null)
            {
                return NotFound();
            }

            await _lookupVehicleStatusManager.DeleteLookupVehicleStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}