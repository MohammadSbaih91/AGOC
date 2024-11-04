using AGOC.ViewModels;

namespace AGOC.Services.Interface
{
    public interface IUserAuthenticationService
    {
        Task<AuthValidationResult> ValidateUserAsync(string username, string password);
    }
}
