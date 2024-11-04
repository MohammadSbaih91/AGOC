namespace AGOC.ViewModels
{
    public class VehicleStatusViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }

        public string Status { get; set; } = null!;

        public DateOnly StatusDate { get; set; }

        public int? LookupVehicleStatusId { get; set; }
    }
}