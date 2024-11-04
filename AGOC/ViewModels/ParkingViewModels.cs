namespace AGOC.ViewModels
{
    public class ParkingViewModels
    {
        public int Id { get; set; }

        public int? VehicleId { get; set; }

        public int EmployeeId { get; set; }

        public string ParkingSpotNumber { get; set; } = null!;

        public DateTime? Date { get; set; }
        public string LicensePlateNumber { get; set; } = null!;
        public int EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }
    }
}