namespace AGOC.ViewModels
{
    public class MessageRecipientViewModel
    {
        public int EmployeeID { get; set; } // Aligned with EmployeeInfo.EmployeeID
        public string Mobile { get; set; } // Aligned with EmployeeInfo.Mobile
        // Optional fields for additional recipient information if needed
        public string EmployeeName { get; set; } // Recipient's name
        public string DepartmentName { get; set; } // Recipient's department
        public string JobTitle { get; set; } // Recipient's job title
        public int StatusID { get; set; } // Foreign key to MessageStatus

    }
}
