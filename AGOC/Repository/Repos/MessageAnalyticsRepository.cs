using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageAnalyticsRepository : RepositoryBase<MessageAnalytics>, IMessageAnalyticsRepository
    {
        public MessageAnalyticsRepository(VehicleMsContext context) : base(context) { }
    }
}
