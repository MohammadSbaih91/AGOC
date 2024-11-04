using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class EmployeeInfoRepository : RepositoryBase<EmployeeInfo>, IEmployeeInfoRepository
    {
        public EmployeeInfoRepository(VehicleMsContext context) : base(context) { }
    }
}
