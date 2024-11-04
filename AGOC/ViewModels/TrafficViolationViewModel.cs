using AGOC.Models;

namespace AGOC.ViewModels
{
    public class TrafficViolationViewModel
    {
        public int Id { get; set; }

        public int? VehicleId { get; set; }

        public int? EmployeeId { get; set; }

        public string? LicensePlateNumber { get; set; }

        public int? EmployeeNumber { get; set; }

        public string? ViolationType { get; set; }

        public int? IsPaid { get; set; }

        public DateOnly? ViolationDate { get; set; }

        public decimal FineAmount { get; set; }


        public int? LookupViolationTypeId { get; set; }
        public virtual LookupViolationType? LookupViolationType { get; set; }
    }
}