using AGOC.Models;
using AGOC.Services;

namespace AGOC.ViewModels
{
    public class VehiclesLookupDetaileIndexViewModel
    {
        public Pagination<VehiclesLookupDetaile> PaginatedVehiclesDetails { get; set; }
        public VehiclesLookupDetaile CreateVehicleDetailModel { get; set; }
    }
}