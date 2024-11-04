using System.Linq.Expressions;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Services
{
    public class HelpersClass
    {
        private readonly IUnitOfWork _unitOfWork;

        public HelpersClass(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool VehicleExists(Expression<Func<Vehicle, bool>> predicate)
        {
            return _unitOfWork.VehicleRepository.Any(predicate);
        }

        public bool VehicleExists(int id, string plateNumber, bool? isDeleted)
        {
            // Check for existing vehicles with the same serial number and plate number
            //bool serialNumberExists;
            bool licensePlateNumberExists;

            if (id == 0)
            {
                // For new vehicles (id == 0), check for any existing vehicle with the same serial number or plate number that is not marked as deleted
                //serialNumberExists = _unitOfWork.VehicleRepository.Any(v => v.SerialNumber == serialNumber && v.IsDeleted == isDeleted);
                licensePlateNumberExists = _unitOfWork.VehicleRepository.Any(v => v.LicensePlateNumber == plateNumber && v.IsDeleted == isDeleted);
            }
            else
            {
                // For updating an existing vehicle, ensure no other vehicle (excluding the current one) has the same serial number or plate number
                //serialNumberExists = _unitOfWork.VehicleRepository.Any(v => v.SerialNumber == serialNumber && v.Id != id && v.IsDeleted == isDeleted);
                licensePlateNumberExists = _unitOfWork.VehicleRepository.Any(v => v.LicensePlateNumber == plateNumber && v.Id != id && v.IsDeleted == isDeleted);
            }

            // If either the serial number or the plate number already exists, return true
            return  licensePlateNumberExists;
        }
    }
}