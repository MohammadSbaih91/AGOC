using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class MessageAnalytics
    {
        [Key]  // Explicitly marking AnalyticsID as the primary key
        public int AnalyticsID { get; set; }
        public int MessageID { get; set; }
        public int SentCount { get; set; }
        public int DeliveredCount { get; set; }
        public int FailedCount { get; set; }
        public double DeliveryRate { get; set; }
        public double FailureRate { get; set; }

        public virtual Message Message { get; set; }
    }
}
