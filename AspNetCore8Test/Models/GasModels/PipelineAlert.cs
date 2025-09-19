using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 管線警報模型
    /// </summary>
    public class PipelineAlert
    {
        public int Id { get; set; }
        
        public int PipelineId { get; set; }
        
        public DateTime AlertTime { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string AlertType { get; set; } = string.Empty; // Pressure, Flow, Temperature, Leak, Maintenance
        
        [StringLength(50)]
        public string Severity { get; set; } = string.Empty; // Low, Medium, High, Critical
        
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Acknowledged, Resolved, Cancelled
        
        public DateTime? AcknowledgedTime { get; set; }
        
        [StringLength(100)]
        public string AcknowledgedBy { get; set; } = string.Empty;
        
        public DateTime? ResolvedTime { get; set; }
        
        [StringLength(100)]
        public string ResolvedBy { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Resolution { get; set; } = string.Empty;
        
        public bool IsNotificationSent { get; set; } = false;
        
        [StringLength(500)]
        public string NotificationRecipients { get; set; } = string.Empty;
        
        // 導航屬性
        public virtual Pipeline Pipeline { get; set; } = null!;
    }
}