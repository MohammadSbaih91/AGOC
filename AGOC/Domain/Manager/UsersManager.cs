using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AGOC.Domain
{
    public class UsersManager : IUsersManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Role> GetRoles(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var roles = await _unitOfWork.Roles.GetByCondition(x => x.Id == id).SingleOrDefaultAsync();
                return roles;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetUser(string userName)
        {
            try
            {
                if (userName == null)
                {
                    throw new ArgumentNullException(nameof(userName));
                }

                return await _unitOfWork.User.GetByCondition(u => u.Username == userName)
                                              .Include(u => u.UserRoles)
                                              .ThenInclude(ur => ur.Role)
                                              .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>> GetUserRoles(int userId)
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                var userRoles = await _unitOfWork.UserRoles.GetByCondition(
                               (ur => ur.UserId == userId))
                               .Select(ur => ur.Role.RoleDescription)
                               .ToListAsync();
                return userRoles;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}