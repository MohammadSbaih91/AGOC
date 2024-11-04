using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageTemplate
    {
        [Key]  // Explicitly marking TemplateID as the primary key
        public int TemplateID { get; set; }

        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string TemplateType { get; set; }  // e.g., "SMS" or "Email"
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
