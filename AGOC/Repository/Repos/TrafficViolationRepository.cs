using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class TrafficViolationRepository : RepositoryBase<TrafficViolation>, ITrafficViolationRepository
    {
        public TrafficViolationRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}