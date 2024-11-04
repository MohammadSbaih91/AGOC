using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class LookupViolationTypeRepository : RepositoryBase<LookupViolationType>, ILookupViolationTypeRepository
    {
        public LookupViolationTypeRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}