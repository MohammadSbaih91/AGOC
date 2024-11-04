namespace AGOC.ViewModels
{
    public class MessageViewModel
    {
        public int MessageID { get; set; } // Unique identifier for the message
        public string MessageContent { get; set; } // The content of the message

        public int? TemplateID { get; set; } // Optional ID for message templates
        public string SendType { get; set; } // Type of send (e.g., "Manual" or "Template-based")
        public string Channel { get; set; } // Channel for message delivery (e.g., SMS, Email)

        public DateTime? ScheduledSendTime { get; set; } // Scheduled time for sending the message
        public DateTime CreatedOn { get; set; } // Creation timestamp
        public string CreatedBy { get; set; } // User who created the message

        public IEnumerable<MessageRecipientViewModel> Recipients { get; set; } = new List<MessageRecipientViewModel>(); // List of message recipients
    }
}
