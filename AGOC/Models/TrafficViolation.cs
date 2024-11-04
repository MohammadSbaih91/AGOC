using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class TrafficViolation
{
    public int Id { get; set; }

    public int? VehicleId { get; set; }

    public int? EmployeeId { get; set; }

    public string? LicensePlateNumber { get; set; }

    public int? EmployeeNumber { get; set; }

    public string? ViolationType { get; set; }

    public DateOnly? ViolationDate { get; set; }

    public decimal? FineAmount { get; set; }

    public int? LookupViolationTypeId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ModifiedBy { get; set; }

    public int? IsPaid { get; set; }

    public virtual LookupViolationType? LookupViolationType { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
