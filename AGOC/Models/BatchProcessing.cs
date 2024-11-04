using System.ComponentModel.DataAnnotations;

namespace AGOC.Models
{
    public class BatchProcessing
    {
        [Key]  // Marking BatchID as the primary key
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public string Description { get; set; }
        public int TotalMessages { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
    }
}
