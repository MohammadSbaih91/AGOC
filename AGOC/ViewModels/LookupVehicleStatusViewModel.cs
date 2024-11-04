namespace AGOC.ViewModels
{
    public class LookupVehicleStatusViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }
    }
}