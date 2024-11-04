using AGOC.Domain.Interfaces;
using AGOC.Models;
using Microsoft.Extensions.Logging;

namespace AGOC.Utilities
{
    public class GetUserForLog
    {
        private readonly IUsersManager _usersManager;
        private readonly ILogger<GetUserForLog> _logger;

        public GetUserForLog(IUsersManager usersManager, ILogger<GetUserForLog> logger)
        {
            _usersManager = usersManager;
            _logger = logger;
        }

        public async Task<int?> GetUserIdByUsernameAsync(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("GetUserIdByUsernameAsync was called with a null or empty username.");
                return null;
            }

            try
            {
                _logger.LogInformation("Attempting to fetch user ID for username: {UserName}", userName);
                User? user = await _usersManager.GetUser(userName);

                if (user == null)
                {
                    _logger.LogWarning("No user found for username: {UserName}", userName);
                    return null;
                }

                _logger.LogInformation("User ID {UserId} retrieved for username: {UserName}", user.Id, userName);
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user ID for username: {UserName}", userName);
                throw;
            }
        }
    }
}