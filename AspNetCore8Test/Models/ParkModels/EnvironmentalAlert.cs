using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 環境警報記錄模型
/// </summary>
public class EnvironmentalAlert
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("EnvironmentalMonitoring")]
    public int MonitoringId { get; set; }

    [Required]
    public AlertType AlertType { get; set; }

    [Required]
    public AlertLevel AlertLevel { get; set; }

    [Required]
    [StringLength(200)]
    public string Message { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime TriggeredAt { get; set; }

    /// <summary>
    /// 警報解除時間
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 是否已處理
    /// </summary>
    public bool IsResolved { get; set; } = false;

    /// <summary>
    /// 處理人員
    /// </summary>
    [StringLength(100)]
    public string ResolvedBy { get; set; } = string.Empty;

    /// <summary>
    /// 處理備註
    /// </summary>
    [StringLength(1000)]
    public string ResolvedNotes { get; set; } = string.Empty;

    /// <summary>
    /// 是否已通知相關人員
    /// </summary>
    public bool IsNotified { get; set; } = false;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 關聯的環境監測記錄
    /// </summary>
    public virtual EnvironmentalMonitoring EnvironmentalMonitoring { get; set; } = null!;
}

/// <summary>
/// 警報類型
/// </summary>
public enum AlertType
{
    /// <summary>
    /// 空氣品質
    /// </summary>
    AirQuality = 1,

    /// <summary>
    /// 水質
    /// </summary>
    WaterQuality = 2,

    /// <summary>
    /// 噪音
    /// </summary>
    Noise = 3,

    /// <summary>
    /// 溫度
    /// </summary>
    Temperature = 4,

    /// <summary>
    /// 濕度
    /// </summary>
    Humidity = 5,

    /// <summary>
    /// 紫外線
    /// </summary>
    UV = 6,

    /// <summary>
    /// 風速
    /// </summary>
    Wind = 7,

    /// <summary>
    /// 降雨
    /// </summary>
    Rain = 8
}

/// <summary>
/// 警報等級
/// </summary>
public enum AlertLevel
{
    /// <summary>
    /// 資訊
    /// </summary>
    Info = 1,

    /// <summary>
    /// 警告
    /// </summary>
    Warning = 2,

    /// <summary>
    /// 危險
    /// </summary>
    Danger = 3,

    /// <summary>
    /// 緊急
    /// </summary>
    Critical = 4
}