using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageLog
    {
        [Key]  // Explicitly marking LogID as the primary key
        public int LogID { get; set; }
        public int RecipientID { get; set; }
        public DateTime EventTime { get; set; } = DateTime.Now;
        public string EventType { get; set; }  // e.g., "Sent", "Failed", "Delivered"
        public string? ErrorMessage { get; set; }

        public virtual MessageRecipient Recipient { get; set; }
    }
}
