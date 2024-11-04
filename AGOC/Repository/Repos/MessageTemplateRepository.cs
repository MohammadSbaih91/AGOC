using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageTemplateRepository : RepositoryBase<MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(VehicleMsContext context) : base(context) { }
    }
}
