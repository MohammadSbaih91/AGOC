using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Managers
{
    public class TrafficViolationManager : ITrafficViolationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HrService _hrService;
        private readonly ISmslogManager _mslogManager;
        private readonly IMapper _mapper;

        public TrafficViolationManager(IUnitOfWork unitOfWork, HrService hrService, ISmslogManager mslogManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _hrService = hrService;
            _mslogManager = mslogManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrafficViolation>> GetAllTrafficViolationsAsync()
        {
            try
            {
                var allTrafficViolations = await _unitOfWork.TrafficViolationRepository.GetByCondition(t => t.IsDeleted == false).ToListAsync();

                return allTrafficViolations;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all traffic violations", ex);
            }
        }

        public async Task<TrafficViolation> GetTrafficViolationByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.TrafficViolationRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting traffic violation by ID {id}", ex);
            }
        }

        public async Task<OperationResult> AddTrafficViolationAsync(TrafficViolation trafficViolation)
        {
            var result = new OperationResult();

            try
            {
                var empId = await _hrService.GetEmployeeByCodeAsync(trafficViolation.EmployeeNumber);
                if (empId == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "Employee not found.";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                var vehicleId = await _unitOfWork.VehicleRepository
                    .GetByCondition(v => v.LicensePlateNumber == trafficViolation.LicensePlateNumber)
                    .Select(e => e.Id)
                    .FirstOrDefaultAsync();

                if (vehicleId == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Vehicle not found.";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                var violationtypeText = await _unitOfWork.LookupViolationTypeRepository
                    .GetByCondition(t => t.Id == trafficViolation.LookupViolationTypeId)
                    .Select(e => e.ViolationType)
                    .FirstOrDefaultAsync();

                trafficViolation.ViolationType = violationtypeText;
                trafficViolation.EmployeeId = empId.EmployeeID;
                trafficViolation.VehicleId = vehicleId;
                trafficViolation.IsDeleted = false;
                trafficViolation.CreatedOn = DateTime.Now;
                trafficViolation.IsPaid = 0;

                await _unitOfWork.TrafficViolationRepository.AddAsync(trafficViolation);
                await _unitOfWork.Save();

                var messageTextBuilder = new StringBuilder();
                messageTextBuilder.Append("تمت اضافة مخالفة لرقم الموظف ")
                                  .Append(empId.EmployeeCode + " ")
                                  .Append("")
                                  .Append("بتاريخ" + " " + trafficViolation.ViolationDate + " ")
                                  .Append(" - نوع المخالفة: ")
                                  .Append(violationtypeText)
                                  .Append(" مبلغ الغرامة: " + " ")
                                  .Append(" " + trafficViolation.FineAmount + " ")
                                  .Append(" ريال" + " ")
                                  .Append("يرجى سداد المخالفة خلال 30 يوم للأنتفاع بالخصم 50%" + "\n")
                                  .Append("مع تحيات قسم الخدمات الإدارية - إدارة الشؤون المالية و الإدارية");
                string messageText = messageTextBuilder.ToString();
                int employeeCode = int.Parse(empId.EmployeeCode);

                try
                {
                    await _mslogManager.SendSmsAndLogAsync("QPP", "jdaephdfklpkfnlbjb", empId.Mobile, messageText, true, 1, employeeCode);
                }
                catch (Exception smsEx)
                {
                    result.Success = false;
                    result.ErrorMessage = "Traffic violation added successfully, but failed to send SMS.";
                    Log.Error(smsEx, "Failed to send SMS for traffic violation {ViolationId}", trafficViolation.Id);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "An error occurred while adding a new traffic violation.";
                Log.Error(ex, "An error occurred while adding a new traffic violation");
            }

            return result;
        }

        public async Task UpdateTrafficViolationAsync(TrafficViolation trafficViolation)
        {
            try
            {
                var violationtypeText = await _unitOfWork.LookupViolationTypeRepository.GetByCondition(t => t.Id == trafficViolation.LookupViolationTypeId)
                   .Select(e => e.ViolationType).FirstOrDefaultAsync();
                trafficViolation.ViolationType = violationtypeText;
                trafficViolation.ModifiedOn = DateTime.Now;
                _unitOfWork.TrafficViolationRepository.Update(trafficViolation);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the traffic violation", ex);
            }
        }

        public async Task DeleteTrafficViolationAsync(int id)
        {
            try
            {
                var trafficViolation = await GetTrafficViolationByIdAsync(id);
                if (trafficViolation != null)
                {
                    trafficViolation.IsDeleted = true;
                    _unitOfWork.TrafficViolationRepository.Update(trafficViolation);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the traffic violation with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<TrafficViolationViewModel>> SearchTrafficViolationsAsync(string searchText, int? employeeNumber, DateTime? violationDate, int? isPaid)
        {
            // Get all traffic violations that are not deleted
            var trafficViolations = _unitOfWork.TrafficViolationRepository.GetByCondition(t => t.IsDeleted == false);

            // Search by Employee Number or License Plate if searchText is provided
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                trafficViolations = trafficViolations.Where(t =>
                    t.EmployeeNumber.ToString().Contains(searchText) ||
                    t.LicensePlateNumber.Contains(searchText));
            }
            if (employeeNumber.HasValue)
            {
                trafficViolations = trafficViolations.Where(t => t.EmployeeNumber == employeeNumber.Value);
            }
            if (violationDate.HasValue)
            {
                trafficViolations = trafficViolations.Where(t => t.ViolationDate == DateOnly.FromDateTime(violationDate.Value));
            }
            if (isPaid.HasValue)
            {
                trafficViolations = trafficViolations.Where(t => t.IsPaid == isPaid.Value);
            }
            var trafficViolationsList = await trafficViolations.ToListAsync();

            return _mapper.Map<IEnumerable<TrafficViolationViewModel>>(trafficViolationsList);
        }

        public async Task PayTrafficViolationAsync(TrafficViolation trafficViolation)
        {
            if (trafficViolation != null)
            {
                trafficViolation.IsPaid = 1;
                trafficViolation.ModifiedOn = DateTime.Now;
                _unitOfWork.TrafficViolationRepository.Update(trafficViolation);
                await _unitOfWork.Save();
            }
        }
    }
}