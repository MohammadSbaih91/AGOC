using AGOC.Models;
using AGOC.Repository.Interfaces;
namespace AGOC.Repository.Repos
{
    public class BatchProcessingRepository : RepositoryBase<BatchProcessing>, IBatchProcessingRepository
    {
        public BatchProcessingRepository(VehicleMsContext context) : base(context) { }
    }
}
