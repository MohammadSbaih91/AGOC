using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public string? SerialNumber { get; set; }

    public string? Model { get; set; }

    public int? ModelId { get; set; }

    public string? Brand { get; set; }

    public int? BrandId { get; set; }

    public string? Color { get; set; }

    public int? VehicleTypeId { get; set; }

    public string? VehicleTypeText { get; set; }

    public string? LicensePlateNumber { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public int? StatusId { get; set; }

    public string? StatusText { get; set; }

    public DateOnly? ConditionDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Year { get; set; }

    public string? Category { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<Parking> Parkings { get; set; } = new List<Parking>();

    public virtual LookupVehicleStatus? Status { get; set; }

    public virtual ICollection<TrafficViolation> TrafficViolations { get; set; } = new List<TrafficViolation>();

    public virtual ICollection<VehicleHandover> VehicleHandovers { get; set; } = new List<VehicleHandover>();
}
