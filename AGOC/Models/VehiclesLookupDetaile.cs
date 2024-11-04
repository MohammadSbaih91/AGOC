namespace AGOC.Models;

public partial class VehiclesLookupDetaile
{
    public int Id { get; set; }

    public int? VehiclesLookupMainId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? VehiclesLookupMainDescription { get; set; }

    public virtual ICollection<VehicleCategoryLookup> VehicleCategoryLookups { get; set; } = new List<VehicleCategoryLookup>();

    public virtual VehiclesLookupMain? VehiclesLookupMain { get; set; }
}