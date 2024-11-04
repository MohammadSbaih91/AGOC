namespace AGOC.ViewModels
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }
        public int VehicleTypeId { get; set; }
        public int? EmployeeNumber { get; set; }
        public DateTime? ViolationDate { get; set; }
        public int? IsPaid { get; set; }
    }
}