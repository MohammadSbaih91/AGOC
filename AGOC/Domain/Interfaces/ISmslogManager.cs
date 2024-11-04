using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface ISmslogManager
    {
        Task<IEnumerable<Smslog>> GetAllSmslogesAsync();

        Task<Smslog> GetSmslogByIdAsync(int id);

        Task AddSmslogAsync(Smslog smslog);

        Task UpdateSmslogAsync(Smslog smslog);

        Task DeleteSmslogAsync(int id);

        Task<bool> SendSmsAndLogAsync(string applicationId, string password, string mobileNumber, string messageText, bool confirmDelivery, int priority, int empCode);
    }
}