using ServiceReference1;

namespace AGOC.Services.Interface
{
    public interface ISmsService
    {
        Task<SMS> SMSPushAsync(string applicationId, string password, string mobileNumber, string messageText, bool confirmDelivery, int priority);
    }
}