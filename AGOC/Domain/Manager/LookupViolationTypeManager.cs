using Microsoft.Extensions.Caching.Memory;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Domain.Manager
{
    public class LookupViolationTypeManager : ILookupViolationTypeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public LookupViolationTypeManager(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task AddLookupViolationTypeAsync(LookupViolationType lookupViolationType)
        {
            try
            {
                await _unitOfWork.LookupViolationTypeRepository.AddAsync(lookupViolationType);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new Lookup Violation Type", ex);
            }
        }

        public async Task DeleteLookupViolationType(int id)
        {
            try
            {
                var lookupViolationType = _unitOfWork.LookupViolationTypeRepository.GetByCondition(v => v.Id == id).SingleOrDefault();
                if (lookupViolationType != null)
                {
                    _unitOfWork.LookupViolationTypeRepository.Delete(lookupViolationType);
                    await _unitOfWork.Save();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the Lookup Violation Type with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<LookupViolationType>> GetAllLookupViolationTypeAsync()
        {
            try
            {
                // Define a cache key
                var cacheKey = "AllLookupViolationTypes";

                if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<LookupViolationType>? lookupViolationTypes))
                {
                    lookupViolationTypes = await _unitOfWork.LookupViolationTypeRepository.GetAllAsync();
                    if (lookupViolationTypes != null && lookupViolationTypes.Any())
                    {
                        // Set cache options
                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                        // Save data in cache
                        _memoryCache.Set(cacheKey, lookupViolationTypes, cacheOptions);
                    }
                    else
                    {
                        // Handle the case where the result is null or empty
                        lookupViolationTypes = Enumerable.Empty<LookupViolationType>();
                    }
                }

                return lookupViolationTypes!;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all Lookup Violation Type", ex);
            }
        }

        public async Task<LookupViolationType> GetLookupViolationTypeByIdAsync(int id)
        {
            try
            {
                var lookupViolationType = await _unitOfWork.LookupViolationTypeRepository.GetByIdAsync(id);
                return lookupViolationType;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting Lookup Violation Type by ID {id}", ex);
            }
        }

        public async Task UpdateLookupViolationType(LookupViolationType lookupViolationType)
        {
            try
            {
                _unitOfWork.LookupViolationTypeRepository.Update(lookupViolationType);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the Lookup Violation Type", ex);
            }
        }
    }
}