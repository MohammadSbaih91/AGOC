namespace AGOC.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRolesRepository Roles { get; }
        IUserRolesRepository UserRoles { get; }
        IUsersRepository User { get; }
        ISmslogRepository SmslogRepository { get; }
        // New repositories
        IMessageRepository MessageRepository { get; }
        IMessageAnalyticsRepository MessageAnalyticsRepository { get; }
        IMessageLogRepository MessageLogRepository { get; }
        IMessageRecipientRepository MessageRecipientRepository { get; }
        IMessageStatusRepository MessageStatusRepository { get; }
        IMessageTemplateRepository MessageTemplateRepository { get; }
        IEmployeeInfoRepository EmployeeInfoRepository { get; }
        public Task<int> Save();
    }
}