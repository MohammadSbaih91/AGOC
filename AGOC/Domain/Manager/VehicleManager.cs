using Microsoft.EntityFrameworkCore;
using AGOC.Domain.Interfaces;
using AGOC.Models;
using AGOC.Repository.Interfaces;
using AGOC.Services;

namespace AGOC.Domain
{
    public class VehicleManager : IVehicleManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HelpersClass _helpersClass;

        public VehicleManager(IUnitOfWork unitOfWork, HelpersClass helpersClass)
        {
            _unitOfWork = unitOfWork;
            _helpersClass = helpersClass;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            try
            {
                var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(id);
                return vehicle;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while getting vehicle by ID {id}", ex);
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                return await _unitOfWork.VehicleRepository.Count(v => v.IsDeleted == false);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the total count of vehicles", ex);
            }
        }

        public async Task<List<Vehicle>> GetPagedVehiclesAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _unitOfWork.VehicleRepository
                    .GetByCondition(v => v.IsDeleted == false)
                    .OrderByDescending(v => v.CreatedOn)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting paginated vehicles", ex);
            }
        }

        public async Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlateNumber)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetByCondition(v => v.LicensePlateNumber == licensePlateNumber
            && v.IsDeleted == false
            && v.VehicleTypeId == 1)
                .SingleOrDefaultAsync();
            return vehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            try
            {
                var allVehicles = await _unitOfWork.VehicleRepository
                    .GetByCondition(v => v.IsDeleted == false)
                    .OrderByDescending(v => v.CreatedOn)
                    .AsNoTracking()
                    .ToListAsync();

                return allVehicles;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting all vehicles", ex);
            }
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            try
            {
                if (vehicle.VehicleTypeId == 1)
                {
                    vehicle.VehicleTypeText = "مستأجرة";
                }
                else if (vehicle.VehicleTypeId == 2)
                {
                    vehicle.VehicleTypeText = "شخصية";
                }
                // Check if the SerialNumber or LicensePlateNumber already exists
                if (_helpersClass.VehicleExists(vehicle.Id, vehicle.LicensePlateNumber, false))
                {
                    throw new ApplicationException("يوجد  مركبة مسجلة مسبقا لنفس رقم اللوحة او رقم الشصي");
                }

                vehicle.IsDeleted = false;
                vehicle.CreatedOn = DateTime.Now;
                await _unitOfWork.VehicleRepository.AddAsync(vehicle);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("يوجد  مركبة مسجلة مسبقا لنفس رقم اللوحة او رقم الشصي", ex);
            }
        }

        public async Task<int> Update(Vehicle vehicle)
        {
            int operationResult = 0;
            try
            {
                if (vehicle.VehicleTypeId == 1)
                {
                    vehicle.VehicleTypeText = "مستأجرة";
                }
                else if (vehicle.VehicleTypeId == 2)
                {
                    vehicle.VehicleTypeText = "شخصية";
                }
                // Check if the SerialNumber or LicensePlateNumber exists for a different vehicle
                if (_helpersClass.VehicleExists(vehicle.Id, vehicle.LicensePlateNumber, vehicle.IsDeleted))
                {
                    throw new ApplicationException("يوجد  مركبة مسجلة مسبقا لنفس رقم اللوحة او رقم الشصي");
                }

                vehicle.IsDeleted = false;
                vehicle.ModifiedOn = DateTime.Now;
                _unitOfWork.VehicleRepository.Update(vehicle);
                await _unitOfWork.Save();
                operationResult = 1;
            }
            catch (Exception ex)
            {
                operationResult = 0;
                throw new ApplicationException("يوجد  مركبة مسجلة مسبقا لنفس رقم اللوحة او رقم الشصي", ex);
            }
            return operationResult;
        }

        public async Task DeleteVehicle(int id)
        {
            try
            {
                var vehicle = _unitOfWork.VehicleRepository.GetByCondition(v => v.Id == id).SingleOrDefault();
                if (vehicle != null)
                {
                    vehicle.IsDeleted = true;
                    _unitOfWork.VehicleRepository.Update(vehicle);
                    await _unitOfWork.Save();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the vehicle with ID {id}", ex);
            }
        }

        public async Task<bool> CheckHandoveredVehicles(int id)
        {
            try
            {
                var handoveredVehicles = _unitOfWork.VehicleHandoverRepository.Any(h => h.VehicleId == id && h.IsDeleted == false);
                return handoveredVehicles; // Returns true if the vehicle is handed over
            }
            catch (Exception ex)
            {
                throw new ApplicationException("حدث خطأ في العملية", ex);
            }
        }

        public async Task<bool> HasTrafficViolations(int id)
        {
            try
            {
                var violations = _unitOfWork.TrafficViolationRepository.Any(h => h.VehicleId == id && h.IsDeleted == false);
                return violations; // Returns true if the vehicle is handed over
            }
            catch (Exception ex)
            {
                throw new ApplicationException("حدث خطأ في العملية", ex);
            }
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string searchText, int vehicleTypeId)
        {
            var query = _unitOfWork.VehicleRepository.GetByCondition(v => v.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(v => v.LicensePlateNumber.Contains(searchText) || v.Brand.Contains(searchText));
            }

            if (vehicleTypeId > 0)
            {
                query = query.Where(v => v.VehicleTypeId == vehicleTypeId);
            }

            return await query.ToListAsync();
        }
    }
}