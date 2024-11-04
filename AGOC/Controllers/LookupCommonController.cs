using Microsoft.AspNetCore.Mvc;
using AGOC.Domain.Interfaces;

namespace AGOC.Controllers
{
    public class LookupCommonController : Controller
    {
        private readonly IVehiclesLookupDetaileManager _vehiclesLookupDetaileManager;
        private readonly IVehiclesLookupMainManager _vehiclesLookupMainManager;
        private readonly ILookupViolationTypeManager _lookupViolationTypeManager;
        private readonly IVehicleCategoryLookupManager _vehicleCategoryLookupManager;
        private readonly ILookupVehicleStatusManager _vehicleStatusManager;
        private readonly ILogger<LookupCommonController> _logger;

        public LookupCommonController(IVehiclesLookupDetaileManager vehiclesLookupDetaileManager, IVehiclesLookupMainManager vehiclesLookupMainManager,
            ILookupViolationTypeManager lookupViolationTypeManager, ILogger<LookupCommonController> logger,
            IVehicleCategoryLookupManager vehicleCategoryLookupManager, ILookupVehicleStatusManager vehicleStatusManager)
        {
            _vehiclesLookupDetaileManager = vehiclesLookupDetaileManager;
            _vehiclesLookupMainManager = vehiclesLookupMainManager;
            _lookupViolationTypeManager = lookupViolationTypeManager;
            _logger = logger;
            _vehicleCategoryLookupManager = vehicleCategoryLookupManager;
            _vehicleStatusManager = vehicleStatusManager;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Navigated to LookupCommon Index page.");
            return View();
        }

        public async Task<IActionResult> GetVehiclesLookupMain()
        {
            try
            {
                _logger.LogInformation("Fetching all vehicle lookup main entries.");
                var lookup = await _vehiclesLookupMainManager.GetAllVehiclesLookupMainesAsync();
                _logger.LogInformation("Successfully fetched vehicle lookup main entries.");
                return Json(lookup.Select(b => new { id = b.Id, description = b.Description }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching vehicle lookup main entries.");
                return StatusCode(500, "An error occurred while fetching data.");
            }
        }

        public async Task<IActionResult> GetVehiclesLookupDetailsByLookMainId(int id)
        {
            try
            {
                _logger.LogInformation("Fetching vehicle lookup details for main ID: {Id}", id);
                var lookup = await _vehiclesLookupDetaileManager.GetVehiclesLookupDetaileByVehiclesLookupMainId(id);
                _logger.LogInformation("Successfully fetched vehicle lookup details for main ID: {Id}", id);
                return Json(lookup.Select(b => new { id = b.Id, description = b.Description }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching vehicle lookup details for main ID: {Id}", id);
                return StatusCode(500, "An error occurred while fetching data.");
            }
        }

        public async Task<IActionResult> GetTrafficViolationTypes()
        {
            try
            {
                _logger.LogInformation("Fetching all traffic violation types.");
                var lookup = await _lookupViolationTypeManager.GetAllLookupViolationTypeAsync();
                var result = lookup.Select(vt => new
                {
                    id = vt.Id,
                    violationType = vt.ViolationType
                });
                _logger.LogInformation("Successfully fetched traffic violation types.");
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching traffic violation types.");
                return StatusCode(500, "An error occurred while fetching data.");
            }
        }

        public async Task<IActionResult> GetLookupVehicleStatus()
        {
            try
            {
                _logger.LogInformation("Fetching all Lookup Vehicle Status");
                var lookup = await _vehicleStatusManager.GetAllLookupVehicleStatusesAsync();
                var result = lookup.Select(vt => new
                {
                    id = vt.Id,
                    status = vt.Status
                });
                _logger.LogInformation("Successfully fetched Lookup Vehicle Status");
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Lookup Vehicle Status.");
                return StatusCode(500, "An error occurred while fetching data.");
            }
        }

        public async Task<IActionResult> GetVehicleCategoryLookups(int id)
        {
            try
            {
                _logger.LogInformation("Fetching Vehicle Category Lookup.");
                var lookup = await _vehicleCategoryLookupManager.GetByVehiclesLookupDetailIdAsync(id);
                _logger.LogInformation("Successfully fetched Vehicle Category Lookup.");
                return Json(lookup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Vehicle Category Lookup.");
                return StatusCode(500, "An error occurred while fetching data.");
            }
        }
    }
}