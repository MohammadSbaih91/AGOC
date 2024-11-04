using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Interfaces
{
    public interface IParkingManager
    {
        Task<OperationResult> AddParkingAsync(ParkingViewModels parkingViewModels);

        Task<OperationResult> DeleteParkingAsync(int id);

        Task<IEnumerable<Parking>> GetAllParkingsAsync();

        Task<Parking> GetParkingByIdAsync(int id);

        Task UpdateParkingAsync(Parking parking);

        Task<IEnumerable<ParkingViewModels>> SearchParking(string searchText);
    }
}