using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class ParkingRepository : RepositoryBase<Parking>, IParkingRepository
    {
        public ParkingRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}