using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.ViewModels;

namespace AGOC.Controllers
{
    [Authorize]
    public class LookupViolationTypeController : Controller
    {
        private readonly ILookupViolationTypeManager _lookupViolationTypeManager;
        private readonly IMapper _mapper;
        private readonly ILogger<LookupViolationTypeController> _logger;

        public LookupViolationTypeController(ILookupViolationTypeManager lookupViolationTypeManager, IMapper mapper, ILogger<LookupViolationTypeController> logger)
        {
            _lookupViolationTypeManager = lookupViolationTypeManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all Lookup Violation Types.");
            var lookupViolationTypes = await _lookupViolationTypeManager.GetAllLookupViolationTypeAsync();
            var mappedModels = _mapper.Map<IEnumerable<LookupViolationTypeViewModel>>(lookupViolationTypes);
            _logger.LogInformation("Successfully retrieved and mapped Lookup Violation Types.");
            return View(mappedModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Fetching details for Lookup Violation Type with ID: {Id}", id);
            var lookupViolationType = await _lookupViolationTypeManager.GetLookupViolationTypeByIdAsync(id);
            if (lookupViolationType == null)
            {
                _logger.LogWarning("Lookup Violation Type with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupViolationTypeViewModel>(lookupViolationType);
            _logger.LogInformation("Successfully retrieved and mapped Lookup Violation Type details for ID: {Id}", id);
            return View(mappedModel);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigated to Create Lookup Violation Type view.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LookupViolationTypeViewModel lookupViolationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to create a new Lookup Violation Type.");
                    var lookupViolationType = _mapper.Map<LookupViolationType>(lookupViolationTypeViewModel);
                    await _lookupViolationTypeManager.AddLookupViolationTypeAsync(lookupViolationType);
                    _logger.LogInformation("Successfully created a new Lookup Violation Type.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating a new Lookup Violation Type.");
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for Lookup Violation Type creation.");
            }

            return View(lookupViolationTypeViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching Lookup Violation Type for editing with ID: {Id}", id);
            var lookupViolationType = await _lookupViolationTypeManager.GetLookupViolationTypeByIdAsync(id);
            if (lookupViolationType == null)
            {
                _logger.LogWarning("Lookup Violation Type with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupViolationTypeViewModel>(lookupViolationType);
            _logger.LogInformation("Successfully retrieved and mapped Lookup Violation Type for editing with ID: {Id}", id);
            return View(mappedModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LookupViolationTypeViewModel updatedLookupViolationType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update Lookup Violation Type with ID: {Id}.", updatedLookupViolationType.Id);
                    var existingLookupViolationType = await _lookupViolationTypeManager.GetLookupViolationTypeByIdAsync(updatedLookupViolationType.Id);
                    if (existingLookupViolationType == null)
                    {
                        _logger.LogWarning("Lookup Violation Type with ID {Id} not found for update.", updatedLookupViolationType.Id);
                        return NotFound();
                    }

                    _mapper.Map(updatedLookupViolationType, existingLookupViolationType);
                    await _lookupViolationTypeManager.UpdateLookupViolationType(existingLookupViolationType);
                    _logger.LogInformation("Successfully updated Lookup Violation Type with ID: {Id}.", updatedLookupViolationType.Id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating Lookup Violation Type with ID: {Id}.", updatedLookupViolationType.Id);
                    ViewData["ErrorMessage"] = ex.Message;
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for Lookup Violation Type update with ID: {Id}.", updatedLookupViolationType.Id);
            }

            return View(updatedLookupViolationType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching Lookup Violation Type for deletion with ID: {Id}", id);
            var lookupViolationType = await _lookupViolationTypeManager.GetLookupViolationTypeByIdAsync(id);
            if (lookupViolationType == null)
            {
                _logger.LogWarning("Lookup Violation Type with ID {Id} not found.", id);
                return NotFound();
            }

            var mappedModel = _mapper.Map<LookupViolationTypeViewModel>(lookupViolationType);
            _logger.LogInformation("Successfully retrieved Lookup Violation Type for deletion with ID: {Id}", id);
            return View(mappedModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Lookup Violation Type with ID: {Id}.", id);
                var existingLookupViolationType = await _lookupViolationTypeManager.GetLookupViolationTypeByIdAsync(id);
                if (existingLookupViolationType == null)
                {
                    _logger.LogWarning("Lookup Violation Type with ID {Id} not found for deletion.", id);
                    return NotFound();
                }

                await _lookupViolationTypeManager.DeleteLookupViolationType(id);
                _logger.LogInformation("Successfully deleted Lookup Violation Type with ID: {Id}.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Lookup Violation Type with ID: {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}