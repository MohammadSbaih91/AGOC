using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageRecipient
    {
        [Key]  // Explicitly marking RecipientID as the primary key
        public int RecipientID { get; set; }

        public int MessageID { get; set; }
        public int EmployeeID { get; set; }
        public int SendStatusID { get; set; }
        public DateTime? SentOn { get; set; }
        public string? ErrorMessage { get; set; }

        public virtual Message Message { get; set; }
        public virtual EmployeeInfo Employee { get; set; }
        public virtual MessageStatus Status { get; set; }
    }
}
