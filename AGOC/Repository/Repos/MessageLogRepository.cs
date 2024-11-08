using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageLogRepository : RepositoryBase<MessageLog>, IMessageLogRepository
    {
        public MessageLogRepository(AGOCContext context) : base(context) { }
    }
}
