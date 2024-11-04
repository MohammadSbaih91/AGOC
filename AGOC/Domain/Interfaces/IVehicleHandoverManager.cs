using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Interfaces
{
    public interface IVehicleHandoverManager
    {
        Task<IEnumerable<VehicleHandoverViewModel>> GetAllVehicleHandoversAsync();

        Task<VehicleHandover> GetVehicleHandoverByIdAsync(int id);

        Task<OperationResult> AddVehicleHandoverAsync(VehicleHandover vehicleHandover);

        Task<OperationResult> CancelVehicleHandoverAsync(int handoverId);

        Task ApproveHandoversAsync(List<int> handoverIds);

        Task DeleteVehicleHandoverAsync(int id);

        Task<IEnumerable<VehicleHandoverViewModel>> SearchHandoversAsync(string searchText);
    }
}