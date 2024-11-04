using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class UserPreference
    {
        [Key]  // Explicitly marking PreferenceID as the primary key
        public int PreferenceID { get; set; }

        public int EmployeeID { get; set; }
        public bool IsOptedIn { get; set; } = true;
        public string PreferredChannel { get; set; }  // "SMS", "Email"
        public string PreferredLanguage { get; set; }  // e.g., "EN", "AR"
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        public virtual EmployeeInfo Employee { get; set; }
    }
}
