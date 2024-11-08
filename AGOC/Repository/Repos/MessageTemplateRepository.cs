using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class MessageTemplateRepository : RepositoryBase<MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(AGOCContext context) : base(context) { }
    }
}
