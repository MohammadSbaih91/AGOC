using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class VehicleCategoryLookupRepository : RepositoryBase<VehicleCategoryLookup>, IVehicleCategoryLookupRepository
    {
        public VehicleCategoryLookupRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}