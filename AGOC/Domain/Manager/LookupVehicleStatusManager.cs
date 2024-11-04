using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Domain.Managers
{
    public class LookupVehicleStatusManager : ILookupVehicleStatusManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupVehicleStatusManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LookupVehicleStatus>> GetAllLookupVehicleStatusesAsync()
        {
            try
            {
                return await _unitOfWork.LookupVehicleStatusRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all lookup vehicle statuses", ex);
            }
        }

        public async Task<LookupVehicleStatus> GetLookupVehicleStatusByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.LookupVehicleStatusRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting lookup vehicle status by ID {id}", ex);
            }
        }

        public async Task AddLookupVehicleStatusAsync(LookupVehicleStatus lookupVehicleStatus)
        {
            try
            {
                await _unitOfWork.LookupVehicleStatusRepository.AddAsync(lookupVehicleStatus);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new lookup vehicle status", ex);
            }
        }

        public async Task UpdateLookupVehicleStatusAsync(LookupVehicleStatus lookupVehicleStatus)
        {
            try
            {
                _unitOfWork.LookupVehicleStatusRepository.Update(lookupVehicleStatus);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the lookup vehicle status", ex);
            }
        }

        public async Task DeleteLookupVehicleStatusAsync(int id)
        {
            try
            {
                var lookupVehicleStatus = await GetLookupVehicleStatusByIdAsync(id);
                if (lookupVehicleStatus != null)
                {
                    _unitOfWork.LookupVehicleStatusRepository.Delete(lookupVehicleStatus);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the lookup vehicle status with ID {id}", ex);
            }
        }
    }
}