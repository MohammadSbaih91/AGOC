using AGOC.Models;
using AGOC.Services;

namespace AGOC.ViewModels
{
    public class VehiclesLookupMainIndexViewModel
    {
        public Pagination<VehiclesLookupMain> PaginatedVehicles { get; set; }
        public VehiclesLookupMain CreateVehicleModel { get; set; }
    }
}
