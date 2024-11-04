using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Domain.Managers
{
    public class VehicleStatusManager : IVehicleStatusManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleStatusManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleStatus>> GetAllVehicleStatusesAsync()
        {
            try
            {
                return await _unitOfWork.VehicleStatusRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all vehicle statuses", ex);
            }
        }

        public async Task<VehicleStatus> GetVehicleStatusByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.VehicleStatusRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting vehicle status by ID {id}", ex);
            }
        }

        public async Task AddVehicleStatusAsync(VehicleStatus vehicleStatus)
        {
            try
            {
                // Check if the associated LookupVehicleStatus exists
                if (vehicleStatus.LookupVehicleStatusId.HasValue)
                {
                    var lookupVehicleStatus = await _unitOfWork.LookupVehicleStatusRepository.GetByIdAsync(vehicleStatus.LookupVehicleStatusId.Value);
                    if (lookupVehicleStatus == null)
                    {
                        throw new ApplicationException($"LookupVehicleStatus with ID {vehicleStatus.LookupVehicleStatusId.Value} does not exist.");
                    }
                }

                await _unitOfWork.VehicleStatusRepository.AddAsync(vehicleStatus);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new vehicle status", ex);
            }
        }

        public async Task UpdateVehicleStatusAsync(VehicleStatus vehicleStatus)
        {
            try
            {
                // Check if the associated LookupVehicleStatus exists
                if (vehicleStatus.LookupVehicleStatusId.HasValue)
                {
                    var lookupVehicleStatus = await _unitOfWork.LookupVehicleStatusRepository.GetByIdAsync(vehicleStatus.LookupVehicleStatusId.Value);
                    if (lookupVehicleStatus == null)
                    {
                        throw new ApplicationException($"LookupVehicleStatus with ID {vehicleStatus.LookupVehicleStatusId.Value} does not exist.");
                    }
                }

                _unitOfWork.VehicleStatusRepository.Update(vehicleStatus);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the vehicle status", ex);
            }
        }

        public async Task DeleteVehicleStatusAsync(int id)
        {
            try
            {
                var vehicleStatus = await GetVehicleStatusByIdAsync(id);
                if (vehicleStatus != null)
                {
                    _unitOfWork.VehicleStatusRepository.Delete(vehicleStatus);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the vehicle status with ID {id}", ex);
            }
        }
    }
}