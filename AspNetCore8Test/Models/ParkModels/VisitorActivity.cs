using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 遊客活動模型
/// </summary>
public class VisitorActivity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public ActivityType Type { get; set; }

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// GPS 座標 - 緯度
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// GPS 座標 - 經度
    /// </summary>
    public decimal? Longitude { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    /// <summary>
    /// 最大參與人數
    /// </summary>
    public int MaxParticipants { get; set; }

    /// <summary>
    /// 目前報名人數
    /// </summary>
    public int CurrentParticipants { get; set; }

    /// <summary>
    /// 費用
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// 主辦單位
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Organizer { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡人
    /// </summary>
    [StringLength(100)]
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡電話
    /// </summary>
    [StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 聯絡信箱
    /// </summary>
    [StringLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 活動狀態
    /// </summary>
    public ActivityStatus Status { get; set; } = ActivityStatus.Scheduled;

    /// <summary>
    /// 是否需要預約
    /// </summary>
    public bool RequiresReservation { get; set; } = true;

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 活動預約記錄
    /// </summary>
    public virtual ICollection<ActivityReservation> Reservations { get; set; } = new List<ActivityReservation>();
}

/// <summary>
/// 活動類型
/// </summary>
public enum ActivityType
{
    /// <summary>
    /// 導覽
    /// </summary>
    GuidedTour = 1,

    /// <summary>
    /// 教育活動
    /// </summary>
    Educational = 2,

    /// <summary>
    /// 運動活動
    /// </summary>
    Sports = 3,

    /// <summary>
    /// 文化活動
    /// </summary>
    Cultural = 4,

    /// <summary>
    /// 自然觀察
    /// </summary>
    NatureWatching = 5,

    /// <summary>
    /// 休閒娛樂
    /// </summary>
    Recreation = 6,

    /// <summary>
    /// 志工活動
    /// </summary>
    Volunteer = 7,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 8
}

/// <summary>
/// 活動狀態
/// </summary>
public enum ActivityStatus
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
    Cancelled = 4,

    /// <summary>
    /// 延期
    /// </summary>
    Postponed = 5
}