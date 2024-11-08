using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private AGOCContext _msContext;

        // Existing repositories
        public IRolesRepository Roles => new RolesRepository(_msContext);
        public IUserRolesRepository UserRoles => new UserRolesRepository(_msContext);
        public IUsersRepository User => new UsersRepository(_msContext);
        public ISmslogRepository SmslogRepository => new SmslogRepository(_msContext);

        // New repositories
        public IMessageRepository MessageRepository => new MessageRepository(_msContext);
        public IMessageAnalyticsRepository MessageAnalyticsRepository => new MessageAnalyticsRepository(_msContext);
        public IMessageLogRepository MessageLogRepository => new MessageLogRepository(_msContext);
        public IMessageRecipientRepository MessageRecipientRepository => new MessageRecipientRepository(_msContext);
        public IMessageStatusRepository MessageStatusRepository => new MessageStatusRepository(_msContext);
        public IMessageTemplateRepository MessageTemplateRepository => new MessageTemplateRepository(_msContext);
        public IEmployeeInfoRepository EmployeeInfoRepository => new EmployeeInfoRepository(_msContext);

        public UnitOfWork(AGOCContext msContext)
        {
            _msContext = msContext;
        }

        public async Task<int> Save()
        {
            return await _msContext.SaveChangesAsync();
        }
    }
}
