using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageAnalyticsRepository : RepositoryBase<MessageAnalytics>, IMessageAnalyticsRepository
    {
        public MessageAnalyticsRepository(AGOCContext context) : base(context) { }
    }
}
