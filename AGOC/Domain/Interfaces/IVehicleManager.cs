using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IVehicleManager
    {
        Task<Vehicle> GetVehicleByIdAsync(int id);

        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        Task AddVehicleAsync(Vehicle vehicle);

        Task DeleteVehicle(int id);

        Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlateNumber);

        Task<bool> CheckHandoveredVehicles(int id);

        Task<bool> HasTrafficViolations(int id);

        Task<int> Update(Vehicle vehicle);

        Task<List<Vehicle>> GetPagedVehiclesAsync(int pageNumber, int pageSize);

        Task<int> GetTotalCountAsync();

        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string searchText, int vehicleTypeId);
    }
}