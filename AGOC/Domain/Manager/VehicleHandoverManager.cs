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
    public class VehicleHandoverManager : IVehicleHandoverManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HrService _herService;
        private readonly ISmslogManager _mslogManager;

        public VehicleHandoverManager(IUnitOfWork unitOfWork, HrService hrService, ISmslogManager mslogManager)
        {
            _unitOfWork = unitOfWork;
            _herService = hrService;
            _mslogManager = mslogManager;
        }

        public async Task<IEnumerable<VehicleHandoverViewModel>> GetAllVehicleHandoversAsync()
        {
            try
            {
                var handovers = await _unitOfWork.VehicleHandoverRepository.GetByCondition(h => h.IsDeleted == false)
                    .Include(h => h.Vehicle) // Make sure to include the Vehicle navigation property
                    .ToListAsync();

                var handoverViewModels = handovers.Select(h => new VehicleHandoverViewModel
                {
                    Id = h.Id,
                    VehicleId = h.VehicleId,
                    EmployeeId = h.EmployeeId,
                    EmployeeCode = h.EmployeeCode,
                    EmployeeName = h.EmployeeName,
                    EmployeeDepartmentId = h.EmployeeDepartmentId,
                    EmployeeDepartment = h.EmployeeDepartment,
                    EmployeeTitle = h.EmployeeTitle,
                    HandoverDate = h.HandoverDate,
                    ReturnDate = h.ReturnDate,
                    Status = h.StatusText,
                    Notes = h.Notes,
                    IsApproved = h.IsApproved,
                    LicensePlateNumber = h.Vehicle?.LicensePlateNumber // Populate LicensePlateNumber
                });

                return handoverViewModels;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all vehicle handovers", ex);
            }
        }

        public async Task<VehicleHandover> GetVehicleHandoverByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.VehicleHandoverRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting vehicle handover by ID {id}", ex);
            }
        }

        public async Task<OperationResult> AddVehicleHandoverAsync(VehicleHandover vehicleHandover)
        {
            var result = new OperationResult();

            try
            {
                // Check if the vehicle exists and is not deleted
                var vehicleExists = _unitOfWork.VehicleRepository.Any(v => v.Id == vehicleHandover.VehicleId && v.IsDeleted == false);
                if (!vehicleExists)
                {
                    result.Success = false;
                    result.ErrorMessage = "لا يمكن تخصيص مركبة غير موجودة او محذوفة";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                // Check if the employee exists
                var employeeExists = await _herService.GetEmployeeByCodeAsync(vehicleHandover.EmployeeCode);
                if (employeeExists == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "لا يمكن تخصيص مركبة لموظف غير موجود";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                // Check if the vehicle is already assigned to the same employee
                var existingActiveHandover = _unitOfWork.VehicleHandoverRepository.Any(h =>
                    h.VehicleId == vehicleHandover.VehicleId &&
                    h.EmployeeCode == vehicleHandover.EmployeeCode &&
                    h.IsDeleted == false);

                if (existingActiveHandover)
                {
                    result.Success = false;
                    result.ErrorMessage = "لا يمكن تخصيص مركبة لنفس الموظف اكثر من مرة";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                // If all checks pass, add the new vehicle handover
                vehicleHandover.IsDeleted = false;
                vehicleHandover.IsApproved = 0;
                vehicleHandover.CreatedOn = DateTime.Now;
                await _unitOfWork.VehicleHandoverRepository.AddAsync(vehicleHandover);
                await _unitOfWork.Save();

                // Prepare SMS message
                var messageTextBuilder = new StringBuilder();
                messageTextBuilder.Append("لقد تم تخصيص المركبة رقم ")
                                  .Append(vehicleHandover.Id)
                                  .Append(" للموظف رقم ")
                                  .Append(employeeExists.EmployeeCode)
                                  .Append(" بنجاح")
                                  .Append(".مع تحيات قسم الخدمات الإدارية - إدارة الشؤون المالية و الإدارية");
                string messageText = messageTextBuilder.ToString();
                int employeeCode = int.Parse(employeeExists.EmployeeCode);

                try
                {
                    await _mslogManager.SendSmsAndLogAsync("", "", employeeExists.Mobile, messageText, true, 1, employeeCode);
                }
                catch (Exception smsEx)
                {
                    result.Success = false;
                    result.ErrorMessage = "Vehicle handover added successfully, but failed to send SMS.";
                    Log.Error(smsEx, "Failed to send SMS for vehicle handover {HandoverId}", vehicleHandover.Id);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "حدث خطأ أثناء محاولة تخصيص المركبة";
                Log.Error(ex, "An error occurred while adding a new vehicle handover");
            }

            return result;
        }

        public async Task<OperationResult> CancelVehicleHandoverAsync(int handoverId)
        {
            var result = new OperationResult();

            try
            {
                var handoverInfo = await GetVehicleHandoverByIdAsync(handoverId);
                if (handoverInfo != null)
                {
                    handoverInfo.IsDeleted = true;
                    handoverInfo.ModifiedOn = DateTime.Now;
                    _unitOfWork.VehicleHandoverRepository.Update(handoverInfo);
                    await _unitOfWork.Save();

                    // Prepare SMS message
                    var empInfo = await _herService.GetEmployeeByCodeAsync(handoverInfo.EmployeeCode);
                    int empCode = int.Parse(empInfo.EmployeeCode);

                    var messageTextBuilder = new StringBuilder();
                    messageTextBuilder.Append("تم إلغاء تخصيص المركبة رقم ")
                                      .Append(handoverInfo.VehicleId)
                                      .Append(" للموظف رقم ")
                                      .Append(empCode)
                                      .Append("مع تحيات قسم الخدمات الإدارية - إدارة الشؤون المالية و الإدارية")
                                      .Append(".");

                    string messageText = messageTextBuilder.ToString();

                    try
                    {
                        // Send SMS and log it
                        await _mslogManager.SendSmsAndLogAsync("", "", empInfo.Mobile, messageText, true, 1, empCode);
                    }
                    catch (Exception smsEx)
                    {
                        result.Success = false;
                        result.ErrorMessage = "Vehicle handover canceled successfully, but failed to send SMS.";
                        Log.Error(smsEx, "Failed to send SMS for vehicle handover cancellation {HandoverId}", handoverId);
                    }

                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "Vehicle handover not found.";
                    Log.Error(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "حدث خطأ أثناء إلغاء تخصيص المركبة";
                Log.Error(ex, "An error occurred while canceling the vehicle handover");
            }

            return result;
        }

        public async Task DeleteVehicleHandoverAsync(int id)
        {
            try
            {
                var vehicleHandover = await GetVehicleHandoverByIdAsync(id);
                if (vehicleHandover != null)
                {
                    _unitOfWork.VehicleHandoverRepository.Delete(vehicleHandover);
                    await _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the vehicle handover with ID {id}", ex);
            }
        }

        public async Task ApproveHandoversAsync(List<int> handoverIds)
        {
            var handovers = await _unitOfWork.VehicleHandoverRepository
                                             .GetByCondition(h => handoverIds.Contains(h.Id))
                                             .ToListAsync();

            foreach (var handover in handovers)
            {
                handover.IsApproved = 1;
                handover.ModifiedOn = DateTime.Now; // Update modification timestamp
            }

            _unitOfWork.VehicleHandoverRepository.Update(handovers);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<VehicleHandoverViewModel>> SearchHandoversAsync(string searchText)
        {
            // Base query for handovers and vehicles that are not deleted
            var handovers = _unitOfWork.VehicleHandoverRepository.GetByCondition(h => h.IsDeleted == false);
            var vehicles = _unitOfWork.VehicleRepository.GetByCondition(v => v.IsDeleted == false);

            // Join between VehicleHandover and Vehicle
            var query = from handover in handovers
                        join vehicle in vehicles on handover.VehicleId equals vehicle.Id
                        select new VehicleHandoverViewModel
                        {
                            Id = handover.Id,
                            VehicleId = handover.VehicleId,
                            EmployeeId = handover.EmployeeId,
                            EmployeeCode = handover.EmployeeCode,
                            EmployeeName = handover.EmployeeName,
                            EmployeeDepartmentId = handover.EmployeeDepartmentId,
                            EmployeeDepartment = handover.EmployeeDepartment,
                            EmployeeTitle = handover.EmployeeTitle,
                            HandoverDate = handover.HandoverDate,
                            ReturnDate = handover.ReturnDate,
                            Status = handover.StatusText,
                            StatusId = handover.StatusId,
                            IsApproved = handover.IsApproved,
                            Notes = handover.Notes,

                            // Vehicle properties
                            LicensePlateNumber = vehicle.LicensePlateNumber
                        };

            // Apply search filtering
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(h =>
                    h.LicensePlateNumber.Contains(searchText) ||
                    h.EmployeeName.Contains(searchText) ||
                    h.EmployeeDepartment.Contains(searchText) ||
                    (h.EmployeeCode.HasValue && h.EmployeeCode.Value.ToString().Contains(searchText)));
            }


            // Execute the query and return the result
            return await query.ToListAsync();
        }

    }
}