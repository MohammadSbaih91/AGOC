using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class EmployeeInfo
    {
        [Key]  // Explicitly marking EmployeeID as the primary key
        public int EmployeeID { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Mobile { get; set; }
        public string EmployeeNameEng { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameEn { get; set; }
        public string SectionName { get; set; }
        public string SectionNameEn { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleEn { get; set; }
        public string PhoneExtension { get; set; }
        public string Account { get; set; }
        public bool? ExcludeInPayroll { get; set; }
        public int EmploymentStatusID { get; set; }
    }
}
