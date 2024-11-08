using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class SmslogRepository : RepositoryBase<Smslog>, ISmslogRepository
    {
        public SmslogRepository(AGOCContext AGOCContext) : base(AGOCContext)
        {
        }
    }
}