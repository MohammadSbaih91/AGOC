using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private VehicleMsContext _msContext;

        // Existing repositories
        public ILookupDepartmentRepository LookupDepartmentRepository => new LookupDepartmentRepository(_msContext);
        public ILookupVehicleStatusRepository LookupVehicleStatusRepository => new LookupVehicleStatusRepository(_msContext);
        public ILookupViolationTypeRepository LookupViolationTypeRepository => new LookupViolationTypeRepository(_msContext);
        public IParkingRepository ParkingRepository => new ParkingRepository(_msContext);
        public ITrafficViolationRepository TrafficViolationRepository => new TrafficViolationRepository(_msContext);
        public IVehicleHandoverRepository VehicleHandoverRepository => new VehicleHandoverRepository(_msContext);
        public IVehicleRepository VehicleRepository => new VehicleRepository(_msContext);
        public IVehicleStatusRepository VehicleStatusRepository => new VehicleStatusRepository(_msContext);
        public IRolesRepository Roles => new RolesRepository(_msContext);
        public IUserRolesRepository UserRoles => new UserRolesRepository(_msContext);
        public IUsersRepository User => new UsersRepository(_msContext);
        public ISmslogRepository SmslogRepository => new SmslogRepository(_msContext);
        public IVehiclesLookupMainRepostitory VehiclesLookupMainRepostitory => new VehiclesLookupMainRepository(_msContext);
        public IVehiclesLookupDetaileRepository VehiclesLookupDetaileRepository => new VehiclesLookupDetaileRepository(_msContext);
        public IVehicleCategoryLookupRepository VehicleCategoryLookupRepository => new VehicleCategoryLookupRepository(_msContext);

        // New repositories
        public IMessageRepository MessageRepository => new MessageRepository(_msContext);
        public IMessageAnalyticsRepository MessageAnalyticsRepository => new MessageAnalyticsRepository(_msContext);
        public IMessageLogRepository MessageLogRepository => new MessageLogRepository(_msContext);
        public IMessageRecipientRepository MessageRecipientRepository => new MessageRecipientRepository(_msContext);
        public IMessageStatusRepository MessageStatusRepository => new MessageStatusRepository(_msContext);
        public IMessageTemplateRepository MessageTemplateRepository => new MessageTemplateRepository(_msContext);
        public IBatchProcessingRepository BatchProcessingRepository => new BatchProcessingRepository(_msContext);
        public IEmployeeInfoRepository EmployeeInfoRepository => new EmployeeInfoRepository(_msContext);

        public UnitOfWork(VehicleMsContext msContext)
        {
            _msContext = msContext;
        }

        public async Task<int> Save()
        {
            return await _msContext.SaveChangesAsync();
        }
    }
}
