using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class VehiclesLookupMain
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<VehiclesLookupDetaile> VehiclesLookupDetailes { get; set; } = new List<VehiclesLookupDetaile>();
}
