using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models
{
    /// <summary>
    /// 簽核歷史記錄
    /// </summary>
    public class ApprovalHistory
    {
        public int Id { get; set; }
        
        public int ApprovalId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ApproverName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Action { get; set; } = string.Empty; // Approved, Rejected, Forwarded, etc.
        
        [StringLength(500)]
        public string? Comments { get; set; }
        
        public DateTime ActionDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string? IpAddress { get; set; }
        
        public virtual Approval Approval { get; set; } = null!;
    }
}