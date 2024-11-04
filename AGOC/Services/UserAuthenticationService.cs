using AGOC.Models;
using AGOC.Services.Interface;
using AGOC.ViewModels;

using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;
using System.Text;

namespace AGOC.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly VehicleMsContext _context;

        public UserAuthenticationService(VehicleMsContext context)
        {
            _context = context;
        }

        public async Task<AuthValidationResult> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return new AuthValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            // Compare the hashed password
            bool isValid = VerifyPassword(password, user.Password);

            return new AuthValidationResult
            {
                IsValid = isValid,
                Roles = isValid ? user.UserRoles.Select(ur => ur.Role.RoleDescription).ToList() : Enumerable.Empty<string>(),
                ErrorMessage = isValid ? string.Empty : "Invalid username or password."
            };
        }

        private bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            // Hash the input password and compare with stored hash
            using (var sha256 = SHA256.Create())
            {
                var inputHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                var inputHash = BitConverter.ToString(inputHashBytes).Replace("-", "").ToLower();

                return inputHash == storedHashedPassword;
            }
        }
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
