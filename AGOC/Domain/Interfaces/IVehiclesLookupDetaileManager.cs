using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface IVehiclesLookupDetaileManager
    {
        Task<IEnumerable<VehiclesLookupDetaile>> GetAllVehiclesLookupDetaileesAsync();

        Task<VehiclesLookupDetaile> GetVehiclesLookupDetaileByIdAsync(int id);
        Task<IEnumerable<VehiclesLookupDetaile>> GetVehiclesLookupDetaileByVehiclesLookupMainId(int id);

        Task AddVehiclesLookupDetaileAsync(VehiclesLookupDetaile vehiclesLookupDetaile);

        Task UpdateVehiclesLookupDetaileAsync(VehiclesLookupDetaile vehiclesLookupDetaile);

        Task DeleteVehiclesLookupDetaileAsync(int id);
         Task<IEnumerable<VehiclesLookupMain>> GetAllVehiclesLookupMainesAsync();
    }
}