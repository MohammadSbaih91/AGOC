using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class VehicleStatus
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly StatusDate { get; set; }

    public int? LookupVehicleStatusId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
