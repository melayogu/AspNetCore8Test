using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 服務請求模型
    /// </summary>
    public class ServiceRequest
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RequestNumber { get; set; } = string.Empty;
        
        public int CustomerId { get; set; }
        
        [StringLength(50)]
        public string RequestType { get; set; } = string.Empty; // New Installation, Repair, Maintenance, Disconnection
        
        [StringLength(50)]
        public string Priority { get; set; } = "Normal"; // Low, Normal, High, Emergency
        
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime RequestDate { get; set; } = DateTime.Now;
        
        public DateTime? ScheduledDate { get; set; }
        
        public DateTime? CompletedDate { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Assigned, In Progress, Completed, Cancelled
        
        [StringLength(100)]
        public string AssignedTo { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string ServiceAddress { get; set; } = string.Empty;
        
        public decimal EstimatedCost { get; set; } = 0;
        
        public decimal ActualCost { get; set; } = 0;
        
        [StringLength(1000)]
        public string WorkNotes { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string CustomerFeedback { get; set; } = string.Empty;
        
        public int CustomerSatisfactionRating { get; set; } = 0; // 1-5 stars
        
        // 導航屬性
        public virtual Customer Customer { get; set; } = null!;
    }
}