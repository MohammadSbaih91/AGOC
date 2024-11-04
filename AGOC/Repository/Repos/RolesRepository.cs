using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Repository.Repos;

namespace AGOC.Repository
{
    public class RolesRepository : RepositoryBase<Role>, IRolesRepository
    {
        public RolesRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}