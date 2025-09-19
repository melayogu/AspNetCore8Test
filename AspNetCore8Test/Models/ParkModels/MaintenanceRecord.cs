using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 維護記錄模型
/// </summary>
public class MaintenanceRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Facility")]
    public int FacilityId { get; set; }

    [Required]
    [StringLength(100)]
    public string MaintenanceType { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public MaintenanceStatus Status { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    [Required]
    [StringLength(100)]
    public string AssignedTo { get; set; } = string.Empty;

    /// <summary>
    /// 維護成本
    /// </summary>
    public decimal Cost { get; set; }

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
    /// 關聯的設施
    /// </summary>
    public virtual Facility Facility { get; set; } = null!;
}

/// <summary>
/// 維護狀態
/// </summary>
public enum MaintenanceStatus
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