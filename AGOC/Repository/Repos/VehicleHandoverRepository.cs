using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class VehicleHandoverRepository : RepositoryBase<VehicleHandover>, IVehicleHandoverRepository
    {
        public VehicleHandoverRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}