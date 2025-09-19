using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 遊客回饋記錄模型
/// </summary>
public class VisitorFeedback
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string VisitorName { get; set; } = string.Empty;

    [StringLength(100)]
    public string VisitorEmail { get; set; } = string.Empty;

    [StringLength(20)]
    public string VisitorPhone { get; set; } = string.Empty;

    [Required]
    public FeedbackType Type { get; set; }

    [Required]
    public FeedbackCategory Category { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 評分 (1-5 星)
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// 回饋狀態
    /// </summary>
    public FeedbackStatus Status { get; set; } = FeedbackStatus.Pending;

    /// <summary>
    /// 處理人員
    /// </summary>
    [StringLength(100)]
    public string HandledBy { get; set; } = string.Empty;

    /// <summary>
    /// 處理時間
    /// </summary>
    public DateTime? HandledAt { get; set; }

    /// <summary>
    /// 處理回覆
    /// </summary>
    [StringLength(2000)]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// 優先級
    /// </summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// 是否公開顯示
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 回饋類型
/// </summary>
public enum FeedbackType
{
    /// <summary>
    /// 建議
    /// </summary>
    Suggestion = 1,

    /// <summary>
    /// 投訴
    /// </summary>
    Complaint = 2,

    /// <summary>
    /// 讚美
    /// </summary>
    Compliment = 3,

    /// <summary>
    /// 問題回報
    /// </summary>
    IssueReport = 4,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 5
}

/// <summary>
/// 回饋分類
/// </summary>
public enum FeedbackCategory
{
    /// <summary>
    /// 設施設備
    /// </summary>
    Facilities = 1,

    /// <summary>
    /// 環境衛生
    /// </summary>
    Cleanliness = 2,

    /// <summary>
    /// 服務品質
    /// </summary>
    Service = 3,

    /// <summary>
    /// 活動安排
    /// </summary>
    Activities = 4,

    /// <summary>
    /// 安全問題
    /// </summary>
    Safety = 5,

    /// <summary>
    /// 交通停車
    /// </summary>
    Transportation = 6,

    /// <summary>
    /// 資訊提供
    /// </summary>
    Information = 7,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 8
}

/// <summary>
/// 回饋狀態
/// </summary>
public enum FeedbackStatus
{
    /// <summary>
    /// 待處理
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 處理中
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// 已解決
    /// </summary>
    Resolved = 3,

    /// <summary>
    /// 已關閉
    /// </summary>
    Closed = 4
}

/// <summary>
/// 優先級
/// </summary>
public enum Priority
{
    /// <summary>
    /// 低
    /// </summary>
    Low = 1,

    /// <summary>
    /// 中
    /// </summary>
    Medium = 2,

    /// <summary>
    /// 高
    /// </summary>
    High = 3,

    /// <summary>
    /// 緊急
    /// </summary>
    Critical = 4
}