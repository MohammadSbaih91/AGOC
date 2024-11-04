using AGOC.Models;

namespace AGOC.ViewModels
{
    public class LookupViolationTypeViewModel
    {
        public int Id { get; set; }

        public string ViolationType { get; set; } = null!;

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }
    }
}