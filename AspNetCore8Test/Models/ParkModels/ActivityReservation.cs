using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 活動預約記錄模型
/// </summary>
public class ActivityReservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("VisitorActivity")]
    public int ActivityId { get; set; }

    [Required]
    [StringLength(100)]
    public string VisitorName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string VisitorPhone { get; set; } = string.Empty;

    [StringLength(100)]
    public string VisitorEmail { get; set; } = string.Empty;

    /// <summary>
    /// 參與人數
    /// </summary>
    public int ParticipantCount { get; set; } = 1;

    /// <summary>
    /// 預約狀態
    /// </summary>
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    /// <summary>
    /// 預約時間
    /// </summary>
    public DateTime ReservationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 確認時間
    /// </summary>
    public DateTime? ConfirmationDate { get; set; }

    /// <summary>
    /// 取消時間
    /// </summary>
    public DateTime? CancellationDate { get; set; }

    /// <summary>
    /// 取消原因
    /// </summary>
    [StringLength(500)]
    public string CancellationReason { get; set; } = string.Empty;

    /// <summary>
    /// 特殊需求
    /// </summary>
    [StringLength(1000)]
    public string SpecialRequirements { get; set; } = string.Empty;

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
    /// 關聯的活動
    /// </summary>
    public virtual VisitorActivity VisitorActivity { get; set; } = null!;
}

/// <summary>
/// 預約狀態
/// </summary>
public enum ReservationStatus
{
    /// <summary>
    /// 待確認
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 已確認
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// 未出席
    /// </summary>
    NoShow = 4,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 5
}