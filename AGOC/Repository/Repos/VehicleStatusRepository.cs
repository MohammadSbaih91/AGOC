using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class VehicleStatusRepository : RepositoryBase<VehicleStatus>, IVehicleStatusRepository
    {
        public VehicleStatusRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}