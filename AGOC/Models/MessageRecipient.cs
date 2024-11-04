using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageRecipient
    {
        [Key]
        public int RecipientID { get; set; }
        public int MessageID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusID { get; set; } // Consolidated status column
        public DateTime? SentOn { get; set; }
        public string? ErrorMessage { get; set; }

        public virtual Message Message { get; set; }
        public virtual EmployeeInfo Employee { get; set; }
        public virtual MessageStatus Status { get; set; }
    }
}
