using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Services;
using AGOC.ViewModels;

namespace AGOC.Domain.Manager
{
    public class ParkingManager : IParkingManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HrService _hrService;
        private readonly ISmslogManager _mslogManager;

        public ParkingManager(IUnitOfWork unitOfWork, IMapper mapper, HrService hrService, ISmslogManager smslogManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hrService = hrService;
            _mslogManager = smslogManager;
        }

        public async Task<OperationResult> AddParkingAsync(ParkingViewModels parkingViewModels)
        {
            var result = new OperationResult();

            try
            {
                //// Get the vehicle ID based on the LicensePlateNumber
                //var vehicleId = _unitOfWork.VehicleRepository.GetByCondition(x => x.LicensePlateNumber == parkingViewModels.LicensePlateNumber)
                //    .AsNoTracking()
                //    .Select(v => v.Id)
                //    .FirstOrDefault();

                // Check if the vehicle is already assigned to a parking spot
                var employeeHasParking = _unitOfWork.ParkingRepository.Any(p =>
                    p.EmployeeCode == parkingViewModels.EmployeeCode && p.IsDeleted == false);
                if (employeeHasParking)
                {
                    result.Success = false;
                    result.ErrorMessage = "لا يمكن تخصيص اكثر من موقف لنفس الموظف";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                // Check if the parking spot is already assigned to another vehicle
                var parkingSpotAssigned = _unitOfWork.ParkingRepository.Any(p =>
                    p.ParkingSpotNumber == parkingViewModels.ParkingSpotNumber && p.IsDeleted == false);
                if (parkingSpotAssigned)
                {
                    result.Success = false;
                    result.ErrorMessage = "لا يمكن تخصيص موقف مخصص مسبقًا";
                    Log.Error(result.ErrorMessage);
                    return result;
                }

                // Get employee information and map to model
                var empInfo = await _hrService.GetEmployeeByCodeAsync(parkingViewModels.EmployeeCode);
                if (empInfo == null)
                {
                    result.Success = false;
                    result.ErrorMessage = "الموظف غير موجود.";
                    Log.Error(result.ErrorMessage);
                    return result;
                }
                parkingViewModels.EmployeeName = empInfo.EmployeeName;
                parkingViewModels.EmployeeId = empInfo.EmployeeID;
                //parkingViewModels.VehicleId = vehicleId;

                // Map the view model to the Parking model
                var mappedModel = _mapper.Map<Parking>(parkingViewModels);
                mappedModel.CreatedOn = DateTime.UtcNow;
                mappedModel.IsDeleted = false;

                // Add the parking record and save changes
                await _unitOfWork.ParkingRepository.AddAsync(mappedModel);
                await _unitOfWork.Save();

                // Send SMS Notification
                var messageTextBuilder = new StringBuilder();
                messageTextBuilder.Append("تم تخصيص موقف رقم ")
                                  .Append(parkingViewModels.ParkingSpotNumber)
                                  .Append(" للمركبة رقم ")
                                  .Append(parkingViewModels.LicensePlateNumber)
                                  .Append(" للموظف رقم ")
                                  .Append(empInfo.EmployeeCode)
                                  .Append("مع تحيات قسم الخدمات الإدارية - إدارة الشؤون المالية و الإدارية")
                                  .Append('.');

                string messageText = messageTextBuilder.ToString();
                int empCode = int.Parse(empInfo.EmployeeCode);

                try
                {
                    await _mslogManager.SendSmsAndLogAsync("", "", empInfo.Mobile, messageText, true, 1, empCode);
                }
                catch (Exception smsEx)
                {
                    result.Success = false;
                    result.ErrorMessage = "Parking assigned successfully, but failed to send SMS.";
                    Log.Error(smsEx, "Failed to send SMS for parking assignment {ParkingSpotNumber}", parkingViewModels.ParkingSpotNumber);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "حدث خطأ أثناء محاولة تخصيص الموقف";
                Log.Error(ex, "An error occurred while adding a new parking assignment");
            }

            return result;
        }

        public async Task<OperationResult> DeleteParkingAsync(int id)
        {
            try
            {
                var result = new OperationResult();

                var parking = _unitOfWork.ParkingRepository.GetByCondition(p => p.Id == id).AsNoTracking().SingleOrDefault();
                if (parking != null)
                {
                    parking.IsDeleted = true;
                    _unitOfWork.ParkingRepository.Update(parking);
                    await _unitOfWork.Save();

                    var empInfo = await _hrService.GetEmployeeByCodeAsync(parking.EmployeeCode);
                    var messageTextBuilder = new StringBuilder();
                    messageTextBuilder.Append("تم  إلغاء تخصيص موقف رقم ")
                                      .Append(parking.ParkingSpotNumber)
                                      .Append(" للمركبة رقم ")
                                      .Append(parking.LicensePlateNumber)
                                      .Append(" للموظف رقم ")
                                      .Append(empInfo.EmployeeCode)
                                      .Append("مع تحيات قسم الخدمات الإدارية - إدارة الشؤون المالية و الإدارية")
                                      .Append('.');

                    string messageText = messageTextBuilder.ToString();
                    int empCode = int.Parse(empInfo.EmployeeCode);

                    try
                    {
                        await _mslogManager.SendSmsAndLogAsync("", "", empInfo.Mobile, messageText, true, 1, empCode);
                    }
                    catch (Exception smsEx)
                    {
                        result.Success = false;
                        result.ErrorMessage = "Parking assigned successfully, but failed to send SMS.";
                        Log.Error(smsEx, "Failed to send SMS for parking assignment {ParkingSpotNumber}", parking.ParkingSpotNumber);
                    }

                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the Parking entry with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Parking>> GetAllParkingsAsync()
        {
            try
            {
                // Retrieve all parking entries that are not deleted
                var parkings = await _unitOfWork.ParkingRepository
                    .GetByCondition(p => p.IsDeleted == false)
                    .AsNoTracking()
                    .ToListAsync();

                //foreach (var parking in parkings)
                //{
                //    // Fetch the employee information for each parking entry
                //    var employeeInfo = await _hrService.GetEmployeeByCodeAsync(parking.EmployeeCode);
                //    if (employeeInfo != null)
                //    {
                //        parking.EmployeeName = employeeInfo.EmployeeName;
                //    }
                //}

                return parkings;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all Parking entries", ex);
            }
        }

        public async Task<Parking> GetParkingByIdAsync(int id)
        {
            try
            {
                var parking = await _unitOfWork.ParkingRepository.GetByCondition(p => p.Id == id).AsNoTracking().SingleOrDefaultAsync();
                return parking;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the Parking entry with ID {id}", ex);
            }
        }

        public async Task UpdateParkingAsync(Parking parking)
        {
            try
            {
                // Check if the vehicle is already assigned to a parking spot with a different Id (excluding the current record)
                var vehicleHasParking = _unitOfWork.ParkingRepository.Any(p =>
                    p.VehicleId == parking.VehicleId && p.Id != parking.Id);

                if (vehicleHasParking)
                {
                    throw new ApplicationException(" المركبة مخصص لها موقف مسبقًا");
                }

                // If no conflicts are found, update the parking entry
                parking.ModifiedOn = DateTime.UtcNow;
                _unitOfWork.ParkingRepository.Update(parking);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException();
            }
        }

        public async Task<IEnumerable<ParkingViewModels>> SearchParking(string searchText)
        {
            try
            {
                var parkingsQuery = _unitOfWork.ParkingRepository.GetByCondition(p => p.IsDeleted == false).AsNoTracking();
                if (!string.IsNullOrEmpty(searchText))
                {
                    parkingsQuery = parkingsQuery.Where(h =>
                        h.ParkingSpotNumber.Contains(searchText) ||
                        h.EmployeeName.Contains(searchText) ||
                        (h.EmployeeCode.HasValue && h.EmployeeCode.Value.ToString().Contains(searchText)));
                }

                var parkings = await parkingsQuery.AsNoTracking().ToListAsync();
                var mappedModel = _mapper.Map<IEnumerable<ParkingViewModels>>(parkings);

                return mappedModel;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving or searching Parking entries", ex);
            }
        }
    }
}