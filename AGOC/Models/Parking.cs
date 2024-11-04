using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class Parking
{
    public int Id { get; set; }

    public int? VehicleId { get; set; }

    public string? LicensePlateNumber { get; set; }

    public int EmployeeId { get; set; }

    public int? EmployeeCode { get; set; }

    public string? EmployeeName { get; set; }

    public string? ParkingSpotNumber { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
