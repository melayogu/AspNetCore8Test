using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 公園事件模型
/// </summary>
public class ParkEvent
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public EventType Type { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 最大參與人數
    /// </summary>
    public int? MaxParticipants { get; set; }

    /// <summary>
    /// 是否需要報名
    /// </summary>
    public bool RegistrationRequired { get; set; }

    /// <summary>
    /// 主辦單位
    /// </summary>
    [StringLength(200)]
    public string Organizer { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡資訊
    /// </summary>
    [StringLength(200)]
    public string ContactInfo { get; set; } = string.Empty;

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// 事件狀態
    /// </summary>
    public EventStatus Status { get; set; } = EventStatus.Scheduled;

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
/// 維護事件模型
/// </summary>
public class MaintenanceEvent
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public MaintenanceType Type { get; set; }

    /// <summary>
    /// 相關設施ID
    /// </summary>
    public int? FacilityId { get; set; }

    /// <summary>
    /// 排程時間
    /// </summary>
    [Required]
    public DateTime ScheduledDate { get; set; }

    /// <summary>
    /// 預估時長（小時）
    /// </summary>
    public decimal? EstimatedDuration { get; set; }

    /// <summary>
    /// 優先級
    /// </summary>
    public MaintenancePriority Priority { get; set; } = MaintenancePriority.Medium;

    /// <summary>
    /// 指派對象
    /// </summary>
    [StringLength(100)]
    public string AssignedTo { get; set; } = string.Empty;

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// 維護狀態
    /// </summary>
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;

    /// <summary>
    /// 完成時間
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// 完成備註
    /// </summary>
    [StringLength(1000)]
    public string CompletionNotes { get; set; } = string.Empty;

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
/// 緊急事件模型
/// </summary>
public class EmergencyEvent
{
    [Key]
    public int Id { get; set; }

    [Required]
    public EmergencyType Type { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public EmergencySeverity Severity { get; set; }

    /// <summary>
    /// 回報人
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ReportedBy { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡資訊
    /// </summary>
    [Required]
    [StringLength(200)]
    public string ContactInfo { get; set; } = string.Empty;

    /// <summary>
    /// 發生時間
    /// </summary>
    [Required]
    public DateTime OccurredAt { get; set; }

    /// <summary>
    /// 回應人員
    /// </summary>
    [StringLength(100)]
    public string RespondedBy { get; set; } = string.Empty;

    /// <summary>
    /// 回應時間
    /// </summary>
    public DateTime? ResponseTime { get; set; }

    /// <summary>
    /// 解決時間
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解決備註
    /// </summary>
    [StringLength(1000)]
    public string ResolutionNotes { get; set; } = string.Empty;

    /// <summary>
    /// 緊急事件狀態
    /// </summary>
    public EmergencyStatus Status { get; set; } = EmergencyStatus.Reported;

    /// <summary>
    /// 影響範圍
    /// </summary>
    [StringLength(200)]
    public string ImpactArea { get; set; } = string.Empty;

    /// <summary>
    /// 預估影響人數
    /// </summary>
    public int? EstimatedAffectedPeople { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

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
/// 事件類型
/// </summary>
public enum EventType
{
    /// <summary>
    /// 節慶活動
    /// </summary>
    Festival = 1,

    /// <summary>
    /// 展覽
    /// </summary>
    Exhibition = 2,

    /// <summary>
    /// 體育活動
    /// </summary>
    Sports = 3,

    /// <summary>
    /// 教育活動
    /// </summary>
    Educational = 4,

    /// <summary>
    /// 社區活動
    /// </summary>
    Community = 5,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 6
}

/// <summary>
/// 事件狀態
/// </summary>
public enum EventStatus
{
    /// <summary>
    /// 已排程
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// 進行中
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 3,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 4
}

/// <summary>
/// 維護類型
/// </summary>
public enum MaintenanceType
{
    /// <summary>
    /// 清潔維護
    /// </summary>
    Cleaning = 1,

    /// <summary>
    /// 設施維修
    /// </summary>
    Repair = 2,

    /// <summary>
    /// 景觀維護
    /// </summary>
    Landscaping = 3,

    /// <summary>
    /// 設備保養
    /// </summary>
    Equipment = 4,

    /// <summary>
    /// 安全檢查
    /// </summary>
    Safety = 5,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 6
}

/// <summary>
/// 維護優先級
/// </summary>
public enum MaintenancePriority
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

/// <summary>
/// 緊急事件類型
/// </summary>
public enum EmergencyType
{
    /// <summary>
    /// 意外事故
    /// </summary>
    Accident = 1,

    /// <summary>
    /// 安全問題
    /// </summary>
    Security = 2,

    /// <summary>
    /// 天氣災害
    /// </summary>
    Weather = 3,

    /// <summary>
    /// 火災
    /// </summary>
    Fire = 4,

    /// <summary>
    /// 醫療緊急
    /// </summary>
    Medical = 5,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 6
}

/// <summary>
/// 緊急事件嚴重程度
/// </summary>
public enum EmergencySeverity
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

/// <summary>
/// 緊急事件狀態
/// </summary>
public enum EmergencyStatus
{
    /// <summary>
    /// 已回報
    /// </summary>
    Reported = 1,

    /// <summary>
    /// 回應中
    /// </summary>
    Responding = 2,

    /// <summary>
    /// 已解決
    /// </summary>
    Resolved = 3
}