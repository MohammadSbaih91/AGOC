using Microsoft.Extensions.Caching.Memory;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Domain.Managers
{
    public class VehiclesLookupMainManager : IVehiclesLookupMainManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public VehiclesLookupMainManager(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<VehiclesLookupMain>> GetAllVehiclesLookupMainesAsync()
        {
            try
            {
                // Get the data directly from the repository without caching
                var vehicleLookupMains = await _unitOfWork.VehiclesLookupMainRepostitory.GetAllAsync();

                // Return an empty collection if no data is found
                return vehicleLookupMains ?? Enumerable.Empty<VehiclesLookupMain>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all lookup vehicle statuses", ex);
            }
        }

        public async Task<VehiclesLookupMain> GetVehiclesLookupMainByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.VehiclesLookupMainRepostitory.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting lookup vehicle status by ID {id}", ex);
            }
        }

        public async Task AddVehiclesLookupMainAsync(VehiclesLookupMain vehiclesLookupMain)
        {
            try
            {
                await _unitOfWork.VehiclesLookupMainRepostitory.AddAsync(vehiclesLookupMain);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new lookup vehicle status", ex);
            }
        }

        public async Task UpdateVehiclesLookupMainAsync(VehiclesLookupMain vehiclesLookupMain)
        {
            try
            {
                _unitOfWork.VehiclesLookupMainRepostitory.Update(vehiclesLookupMain);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the lookup vehicle status", ex);
            }
        }

        public async Task DeleteVehiclesLookupMainAsync(int id)
        {
            try
            {
                var vehiclesLookupMain = await GetVehiclesLookupMainByIdAsync(id);
                if (vehiclesLookupMain != null)
                {
                    _unitOfWork.VehiclesLookupMainRepostitory.Delete(vehiclesLookupMain);
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