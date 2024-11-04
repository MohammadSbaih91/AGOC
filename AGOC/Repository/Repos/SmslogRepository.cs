using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class SmslogRepository : RepositoryBase<Smslog>, ISmslogRepository
    {
        public SmslogRepository(VehicleMsContext vehicleMsContext) : base(vehicleMsContext)
        {
        }
    }
}