using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class LookupVehicleStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
