using AGOC.Models;
namespace AGOC.Domain.Interfaces
{
    public interface IVehicleStatusManager
    {
        Task<IEnumerable<VehicleStatus>> GetAllVehicleStatusesAsync();
        Task<VehicleStatus> GetVehicleStatusByIdAsync(int id);
        Task AddVehicleStatusAsync(VehicleStatus vehicleStatus);
        Task UpdateVehicleStatusAsync(VehicleStatus vehicleStatus);
        Task DeleteVehicleStatusAsync(int id);
    }
}
