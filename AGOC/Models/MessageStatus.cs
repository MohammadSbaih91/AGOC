using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageStatus
    {
        [Key]  // Explicitly marking StatusID as the primary key
        public int StatusID { get; set; }
        public string StatusName { get; set; }  // e.g., "Pending", "Sent", "Failed", etc.
    }
}
