using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IVehicleCategoryLookupManager
    {
        Task<IEnumerable<VehicleCategoryLookup>> GetAllAsync();

        Task<VehicleCategoryLookup?> GetByIdAsync(int id);

        Task AddAsync(VehicleCategoryLookup vehicleCategoryLookup);

        Task UpdateAsync(VehicleCategoryLookup vehicleCategoryLookup);
        Task<IEnumerable<VehicleCategoryLookup>> GetByVehiclesLookupDetailIdAsync(int vehiclesLookupDetailId);

        Task DeleteAsync(int id);
    }
}