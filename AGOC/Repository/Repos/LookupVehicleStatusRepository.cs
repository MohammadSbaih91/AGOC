using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class LookupVehicleStatusRepository : RepositoryBase<LookupVehicleStatus>, ILookupVehicleStatusRepository
    {
        public LookupVehicleStatusRepository(VehicleMsContext VehicleMsContext) : base(VehicleMsContext)
        {
        }
    }
}