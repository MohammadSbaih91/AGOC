namespace AGOC.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        ILookupDepartmentRepository LookupDepartmentRepository { get; }
        ILookupVehicleStatusRepository LookupVehicleStatusRepository { get; }
        ILookupViolationTypeRepository LookupViolationTypeRepository { get; }
        IParkingRepository ParkingRepository { get; }
        ITrafficViolationRepository TrafficViolationRepository { get; }
        IVehicleHandoverRepository VehicleHandoverRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IVehicleStatusRepository VehicleStatusRepository { get; }
        IRolesRepository Roles { get; }
        IUserRolesRepository UserRoles { get; }
        IUsersRepository User { get; }
        ISmslogRepository SmslogRepository { get; }
        IVehiclesLookupMainRepostitory VehiclesLookupMainRepostitory { get; }
        IVehiclesLookupDetaileRepository VehiclesLookupDetaileRepository { get; }
        IVehicleCategoryLookupRepository VehicleCategoryLookupRepository { get; }

        public Task<int> Save();
    }
}