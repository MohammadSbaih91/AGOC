using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageStatusRepository : RepositoryBase<MessageStatus>, IMessageStatusRepository
    {
        public MessageStatusRepository(VehicleMsContext context) : base(context) { }
    }
}
