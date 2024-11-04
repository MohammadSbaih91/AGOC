using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Services.Interface;

namespace AGOC.Domain.Managers
{
    public class SmslogManager : ISmslogManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;

        public SmslogManager(IUnitOfWork unitOfWork, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
        }

        public async Task<IEnumerable<Smslog>> GetAllSmslogesAsync()
        {
            try
            {
                var smslog = await _unitOfWork.SmslogRepository.GetAllAsync();
                smslog.OrderByDescending(x => x.SentOn);
                return smslog;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all SMS logs", ex);
            }
        }

        public async Task<Smslog> GetSmslogByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.SmslogRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting SMS log by ID {id}", ex);
            }
        }

        public async Task AddSmslogAsync(Smslog smslog)
        {
            try
            {
                smslog.CreatedOn = DateTime.Now;
                await _unitOfWork.SmslogRepository.AddAsync(smslog);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new SMS log", ex);
            }
        }

        public async Task UpdateSmslogAsync(Smslog smslog)
        {
            try
            {
                smslog.ModifiedOn = DateTime.Now;
                _unitOfWork.SmslogRepository.Update(smslog);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the SMS log", ex);
            }
        }

        public async Task DeleteSmslogAsync(int id)
        {
            try
            {
                var smslog = await GetSmslogByIdAsync(id);
                if (smslog != null)
                {
                    _unitOfWork.SmslogRepository.Delete(smslog);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the SMS log with ID {id}", ex);
            }
        }

        public async Task<bool> SendSmsAndLogAsync(string applicationId, string password, string mobileNumber, string messageText, bool confirmDelivery, int priority, int empCode)
        {
            try
            {
                var smsResponse = await _smsService.SMSPushAsync(applicationId, password, mobileNumber, messageText, confirmDelivery, priority);

                // Log the SMS details in the database
                var smslog = new Smslog
                {
                    PhoneNumber = mobileNumber,
                    Message = messageText,
                    SentOn = DateTime.UtcNow,
                    Status = smsResponse.Status,
                    ErrorMessage = smsResponse.ReturnCode,
                    EmpolyeeCode = empCode
                };

                await AddSmslogAsync(smslog);

                return smsResponse.Equals(smslog.Status);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while sending SMS and logging the details", ex);
            }
        }
    }
}