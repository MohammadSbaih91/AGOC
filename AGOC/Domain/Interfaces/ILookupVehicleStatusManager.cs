using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface ILookupVehicleStatusManager
    {
        Task<IEnumerable<LookupVehicleStatus>> GetAllLookupVehicleStatusesAsync();

        Task<LookupVehicleStatus> GetLookupVehicleStatusByIdAsync(int id);

        Task AddLookupVehicleStatusAsync(LookupVehicleStatus lookupVehicleStatus);

        Task UpdateLookupVehicleStatusAsync(LookupVehicleStatus lookupVehicleStatus);

        Task DeleteLookupVehicleStatusAsync(int id);
    }
}