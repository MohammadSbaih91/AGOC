using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IUsersManager
    {
        Task<User?> GetUser(string userName);

        Task<Role> GetRoles(int? id);

        Task<List<string>> GetUserRoles(int userId);
    }
}