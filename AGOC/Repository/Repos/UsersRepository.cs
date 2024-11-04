using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Repository.Repos;

namespace AGOC.Repository
{
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        public UsersRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}