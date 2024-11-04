namespace AGOC.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }

        public string? SerialNumber { get; set; }

        public string? Model { get; set; }

        public int? ModelId { get; set; }

        public string? Brand { get; set; }

        public int? BrandId { get; set; }
        public string? Year { get; set; }
        public string? Color { get; set; }
        public string? Category { get; set; }

        public int? CategoryId { get; set; }
        public int? VehicleTypeId { get; set; }

        public string? VehicleTypeText { get; set; }

        public string? LicensePlateNumber { get; set; }

        public DateOnly? PurchaseDate { get; set; }

        public int? StatusId { get; set; }

        public string? StatusText { get; set; }

        public DateOnly? ConditionDate { get; set; }

        public string? Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsRented => VehicleTypeId == 1;
        public bool IsPersonal => VehicleTypeId == 2;
    }
}