namespace AspNetCore8Test.Models.DTOs
{
    /// <summary>
    /// 簽核待辦項目 DTO
    /// </summary>
    public class PendingApprovalDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequestUser { get; set; } = string.Empty;
        public string ApprovalType { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int DaysOverdue => DueDate.HasValue && DateTime.Now > DueDate.Value 
            ? (DateTime.Now - DueDate.Value).Days 
            : 0;
    }

    /// <summary>
    /// 即時流程項目 DTO
    /// </summary>
    public class InProgressApprovalDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequestUser { get; set; } = string.Empty;
        public string CurrentApprover { get; set; } = string.Empty;
        public string ApprovalType { get; set; } = string.Empty;
        public ApprovalStatus Status { get; set; }
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string LastAction { get; set; } = string.Empty;
        public DateTime LastActionDate { get; set; }
    }

    /// <summary>
    /// 簽核歷史項目 DTO
    /// </summary>
    public class ApprovalHistoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequestUser { get; set; } = string.Empty;
        public string ApprovalType { get; set; } = string.Empty;
        public ApprovalStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string FinalApprover { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }

    /// <summary>
    /// 簽核動作請求 DTO
    /// </summary>
    public class ApprovalActionDto
    {
        public int ApprovalId { get; set; }
        public string Action { get; set; } = string.Empty; // "Approve", "Reject", "Forward"
        public string? Comments { get; set; }
        public string? NextApprover { get; set; } // For forwarding
    }
}