using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class MessageRecipientRepository : RepositoryBase<MessageRecipient>, IMessageRecipientRepository
    {
        public MessageRecipientRepository(VehicleMsContext context) : base(context) { }
    }
}
