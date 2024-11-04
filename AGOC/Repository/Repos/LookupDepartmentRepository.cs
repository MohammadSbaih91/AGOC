using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class LookupDepartmentRepository : RepositoryBase<LookupDepartment>, ILookupDepartmentRepository
    {
        public LookupDepartmentRepository(VehicleMsContext VehicleMsContext) : base(VehicleMsContext)
        {
        }
    }
}