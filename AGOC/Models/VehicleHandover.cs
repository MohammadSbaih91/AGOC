using System;
using System.Collections.Generic;

namespace AGOC.Models;

public partial class VehicleHandover
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public int EmployeeId { get; set; }

    public int? EmployeeCode { get; set; }

    public string? EmployeeName { get; set; }

    public int? EmployeeDepartmentId { get; set; }

    public string? EmployeeDepartment { get; set; }

    public string? EmployeeTitle { get; set; }

    public DateOnly? HandoverDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string? StatusText { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    public string? ModifiedBy { get; set; }

    public int? StatusId { get; set; }

    public int? IsApproved { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
