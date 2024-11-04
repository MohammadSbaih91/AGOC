using ServiceReference1;
using AGOC.Services.Interface;
using Microsoft.Extensions.Logging;

namespace AGOC.Services
{
    public class SmsService : ISmsService
    {
        private readonly SMSServiceSoapClient _soapClient;
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _soapClient = new SMSServiceSoapClient(SMSServiceSoapClient.EndpointConfiguration.SMSServiceSoap);
            _logger = logger;
        }

        public async Task<SMS> SMSPushAsync(string applicationId, string password, string mobileNumber, string messageText, bool confirmDelivery, int priority)
        {
            try
            {
                _logger.LogInformation("Sending SMS to {MobileNumber} with message: {MessageText}", mobileNumber, messageText);

                var result = await _soapClient.SMSPushAsync(applicationId, password, mobileNumber, messageText, confirmDelivery, priority);

                _logger.LogInformation("SMS sent successfully to {MobileNumber} with status: {Status}", mobileNumber, result.Status);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending SMS to {MobileNumber}", mobileNumber);
                throw;
            }
        }
    }
}