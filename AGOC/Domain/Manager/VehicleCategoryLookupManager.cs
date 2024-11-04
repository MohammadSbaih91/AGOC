using Microsoft.EntityFrameworkCore;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Services.Managers
{
    public class VehicleCategoryLookupManager : IVehicleCategoryLookupManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleCategoryLookupManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleCategoryLookup>> GetAllAsync()
        {
            try
            {
                var categories = await _unitOfWork.VehicleCategoryLookupRepository.GetAllAsync();
                var details = await _unitOfWork.VehiclesLookupDetaileRepository.GetAllAsync();
                var mains = await _unitOfWork.VehiclesLookupMainRepostitory.GetAllAsync();

                // Perform the join between categories, details, and mains
                var result = from category in categories
                             join detail in details on category.VehiclesLookupDetailId equals detail.Id
                             join main in mains on detail.VehiclesLookupMainId equals main.Id
                             select new VehicleCategoryLookup
                             {
                                 Id = category.Id,
                                 Description = category.Description,
                                 VehiclesLookupDetailId = category.VehiclesLookupDetailId,
                                 VehiclesLookupDetail = new VehiclesLookupDetaile
                                 {
                                     Id = detail.Id,
                                     Description = detail.Description,
                                     VehiclesLookupMainId = detail.VehiclesLookupMainId,
                                     VehiclesLookupMain = new VehiclesLookupMain
                                     {
                                         Id = main.Id,
                                         Description = main.Description // Include the main description
                                     }
                                 }
                             };

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching all vehicle categories", ex);
            }
        }

        public async Task<IEnumerable<VehicleCategoryLookup>> GetByVehiclesLookupDetailIdAsync(int vehiclesLookupDetailId)
        {
            try
            {
                return await _unitOfWork.VehicleCategoryLookupRepository.GetByCondition(v => v.VehiclesLookupDetailId == vehiclesLookupDetailId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while fetching categories for VehiclesLookupDetailId {vehiclesLookupDetailId}", ex);
            }
        }

        public async Task<VehicleCategoryLookup?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.VehicleCategoryLookupRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while fetching the vehicle category with ID {id}", ex);
            }
        }

        public async Task AddAsync(VehicleCategoryLookup vehicleCategoryLookup)
        {
            try
            {
                vehicleCategoryLookup.CreatedOn = DateTime.Now;
                await _unitOfWork.VehicleCategoryLookupRepository.AddAsync(vehicleCategoryLookup);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new vehicle category", ex);
            }
        }

        public async Task UpdateAsync(VehicleCategoryLookup vehicleCategoryLookup)
        {
            try
            {
                vehicleCategoryLookup.ModifiedOn = DateTime.Now;
                _unitOfWork.VehicleCategoryLookupRepository.Update(vehicleCategoryLookup);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the vehicle category", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var vehicleCategoryLookup = await GetByIdAsync(id);
                if (vehicleCategoryLookup != null)
                {
                    _unitOfWork.VehicleCategoryLookupRepository.Delete(vehicleCategoryLookup);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the vehicle category with ID {id}", ex);
            }
        }
    }
}