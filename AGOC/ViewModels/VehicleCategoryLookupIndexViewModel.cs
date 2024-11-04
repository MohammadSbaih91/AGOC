using AGOC.Models;
using AGOC.Services;

namespace AGOC.ViewModels
{
    public class VehicleCategoryLookupIndexViewModel
    {
        public Pagination<VehicleCategoryLookup> PaginatedCategories { get; set; }
        public VehicleCategoryLookup CreateCategoryModel { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
    }
}