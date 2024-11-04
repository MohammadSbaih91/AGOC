namespace AGOC.ViewModels
{
    public class VehicleHandoverViewModel
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

        public int? StatusId { get; set; }
        public int? IsApproved { get; set; }

        public string? Status { get; set; } = null!;

        public string? Notes { get; set; }

        //public virtual Vehicle Vehicle { get; set; } = null!;
        public string? LicensePlateNumber { get; set; }
    }
}