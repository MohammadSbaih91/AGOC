using AGOC.Models;

namespace AGOC.ViewModels
{
    public class VehicleCategoryLookupViewModel
    {
        public int Id { get; set; }

        public int VehiclesLookupDetailId { get; set; }

        public string Description { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual VehiclesLookupDetaile VehiclesLookupDetail { get; set; } = null!;
    }
}