using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class VehiclesLookupMainRepository : RepositoryBase<VehiclesLookupMain>, IVehiclesLookupMainRepostitory
    {
        public VehiclesLookupMainRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}