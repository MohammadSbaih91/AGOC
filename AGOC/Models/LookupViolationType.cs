using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class LookupViolationType
{
    public int Id { get; set; }

    public string ViolationType { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<TrafficViolation> TrafficViolations { get; set; } = new List<TrafficViolation>();
}
