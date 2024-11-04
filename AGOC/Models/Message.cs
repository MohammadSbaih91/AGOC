namespace AGOC.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public string MessageContent { get; set; }
        public int? TemplateID { get; set; }  // Optional FK to MessageTemplate
        public string SendType { get; set; }  // "Manual" or "Template-based"
        public string Channel { get; set; }   // e.g., "SMS", "Email"
        public DateTime? ScheduledSendTime { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        public virtual MessageTemplate? MessageTemplate { get; set; }
        public virtual ICollection<MessageRecipient> Recipients { get; set; } = new List<MessageRecipient>();
    }
}
