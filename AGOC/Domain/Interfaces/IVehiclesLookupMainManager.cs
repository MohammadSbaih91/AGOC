using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IVehiclesLookupMainManager
    {
        Task<IEnumerable<VehiclesLookupMain>> GetAllVehiclesLookupMainesAsync();

        Task<VehiclesLookupMain> GetVehiclesLookupMainByIdAsync(int id);

        Task AddVehiclesLookupMainAsync(VehiclesLookupMain vehiclesLookupMain);

        Task UpdateVehiclesLookupMainAsync(VehiclesLookupMain vehiclesLookupMain);

        Task DeleteVehiclesLookupMainAsync(int id);
    }
}