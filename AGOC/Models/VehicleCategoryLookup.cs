using System.ComponentModel.DataAnnotations;

namespace AGOC.Models;

public partial class VehicleCategoryLookup
{
    public int Id { get; set; }

    public int VehiclesLookupDetailId { get; set; }

    [Required(ErrorMessage = "يرجى إدخال فئة المركبة.")]
    public string Description { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual VehiclesLookupDetaile VehiclesLookupDetail { get; set; } = null!;
}