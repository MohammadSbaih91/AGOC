using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Repository.Repos;

namespace AGOC.Repository
{
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        public UsersRepository(AGOCContext AGOCContext) : base(AGOCContext)
        {
        }
    }
}