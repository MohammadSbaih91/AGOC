using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Domain.Managers
{
    public class VehiclesLookupDetaileManager : IVehiclesLookupDetaileManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public VehiclesLookupDetaileManager(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<VehiclesLookupDetaile>> GetAllVehiclesLookupDetaileesAsync()
        {
            try
            {
                // Perform the join between VehiclesLookupDetaile and VehiclesLookupMain to get related descriptions
                var details =  (from d in await _unitOfWork.VehiclesLookupDetaileRepository.GetAllAsync()
                                     join m in await _unitOfWork.VehiclesLookupMainRepostitory.GetAllAsync()
                                     on d.VehiclesLookupMainId equals m.Id
                                     select new VehiclesLookupDetaile
                                     {
                                         Id = d.Id,
                                         Description = d.Description, // Keep the existing description
                                         VehiclesLookupMainId = d.VehiclesLookupMainId,
                                         VehiclesLookupMainDescription = m.Description // Add the main description
                                     }).ToList();

                // Ensure details is not null
                return details ?? Enumerable.Empty<VehiclesLookupDetaile>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all lookup vehicle statuses", ex);
            }
        }



        public async Task<VehiclesLookupDetaile> GetVehiclesLookupDetaileByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.VehiclesLookupDetaileRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting lookup vehicle status by ID {id}", ex);
            }
        }

        public async Task AddVehiclesLookupDetaileAsync(VehiclesLookupDetaile vehiclesLookupDetaile)
        {
            try
            {
                vehiclesLookupDetaile.CreatedOn = DateTime.Now;
                await _unitOfWork.VehiclesLookupDetaileRepository.AddAsync(vehiclesLookupDetaile);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new lookup vehicle status", ex);
            }
        }

        public async Task UpdateVehiclesLookupDetaileAsync(VehiclesLookupDetaile vehiclesLookupDetaile)
        {
            try
            {
                _unitOfWork.VehiclesLookupDetaileRepository.Update(vehiclesLookupDetaile);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the lookup vehicle status", ex);
            }
        }

        public async Task DeleteVehiclesLookupDetaileAsync(int id)
        {
            try
            {
                var VehiclesLookupDetaile = await GetVehiclesLookupDetaileByIdAsync(id);
                if (VehiclesLookupDetaile != null)
                {
                    _unitOfWork.VehiclesLookupDetaileRepository.Delete(VehiclesLookupDetaile);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the lookup vehicle status with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<VehiclesLookupDetaile>> GetVehiclesLookupDetaileByVehiclesLookupMainId(int id)
        {
            try
            {
                string cacheKey = $"VehiclesLookupDetaile_{id}";

                if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<VehiclesLookupDetaile>? vehiclesDetails))
                {
                    vehiclesDetails = await _unitOfWork.VehiclesLookupDetaileRepository
                        .GetByCondition(v => v.VehiclesLookupMainId == id)
                        .ToListAsync();

                    vehiclesDetails ??= Enumerable.Empty<VehiclesLookupDetaile>();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    };
                    _memoryCache.Set(cacheKey, vehiclesDetails, cacheOptions);
                }

                return vehiclesDetails;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving vehicle lookup details for main ID {id}", ex);
            }
        }

        public async Task<IEnumerable<VehiclesLookupMain>> GetAllVehiclesLookupMainesAsync()
        {
            return await _unitOfWork.VehiclesLookupMainRepostitory.GetAllAsync();
        }
    }
}