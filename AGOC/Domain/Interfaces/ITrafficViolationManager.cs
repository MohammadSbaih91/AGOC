using AGOC.Models;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Interfaces
{
    public interface ITrafficViolationManager
    {
        Task<TrafficViolation> GetTrafficViolationByIdAsync(int id);

        Task<IEnumerable<TrafficViolation>> GetAllTrafficViolationsAsync();

        Task<OperationResult> AddTrafficViolationAsync(TrafficViolation trafficViolation);

        Task UpdateTrafficViolationAsync(TrafficViolation trafficViolation);

        Task DeleteTrafficViolationAsync(int id);
        Task<IEnumerable<TrafficViolationViewModel>> SearchTrafficViolationsAsync(string searchText, int? employeeNumber, DateTime? violationDate, int? isPaid);
        Task PayTrafficViolationAsync(TrafficViolation trafficViolation);
    }
}