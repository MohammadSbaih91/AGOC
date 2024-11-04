using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class VehiclesLookupDetaileRepository : RepositoryBase<VehiclesLookupDetaile>, IVehiclesLookupDetaileRepository
    {
        public VehiclesLookupDetaileRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}