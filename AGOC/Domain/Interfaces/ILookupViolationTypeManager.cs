using AGOC.Models;

namespace AGOC.Domain.Interfaces
{
    public interface ILookupViolationTypeManager
    {
        Task<LookupViolationType> GetLookupViolationTypeByIdAsync(int id);

        Task<IEnumerable<LookupViolationType>> GetAllLookupViolationTypeAsync();

        Task AddLookupViolationTypeAsync(LookupViolationType lookupViolationType);

        Task UpdateLookupViolationType(LookupViolationType lookupViolationType);

        Task DeleteLookupViolationType(int id);
    }
}