using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models
{
    /// <summary>
    /// 簽核項目
    /// </summary>
    public class Approval
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string RequestUser { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string CurrentApprover { get; set; } = string.Empty;
        
        public ApprovalStatus Status { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ApprovalType { get; set; } = string.Empty;
        
        public decimal? Amount { get; set; }
        
        [StringLength(20)]
        public string Priority { get; set; } = "Normal";
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? CompletedDate { get; set; }
        
        [StringLength(500)]
        public string? Remarks { get; set; }
        
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; } = new List<ApprovalHistory>();
    }
}