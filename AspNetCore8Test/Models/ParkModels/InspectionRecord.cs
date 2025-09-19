using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 巡檢記錄模型
/// </summary>
public class InspectionRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Facility")]
    public int FacilityId { get; set; }

    [Required]
    [StringLength(100)]
    public string InspectorName { get; set; } = string.Empty;

    [Required]
    public DateTime InspectionDate { get; set; }

    [Required]
    public InspectionResult Result { get; set; }

    [StringLength(1000)]
    public string Findings { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Recommendations { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要維護
    /// </summary>
    public bool RequiresMaintenance { get; set; }

    /// <summary>
    /// 緊急程度
    /// </summary>
    public UrgencyLevel UrgencyLevel { get; set; }

    /// <summary>
    /// 照片路徑
    /// </summary>
    [StringLength(500)]
    public string PhotoPath { get; set; } = string.Empty;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 關聯的設施
    /// </summary>
    public virtual Facility Facility { get; set; } = null!;
}

/// <summary>
/// 巡檢結果
/// </summary>
public enum InspectionResult
{
    /// <summary>
    /// 良好
    /// </summary>
    Good = 1,

    /// <summary>
    /// 一般
    /// </summary>
    Fair = 2,

    /// <summary>
    /// 需要關注
    /// </summary>
    NeedsAttention = 3,

    /// <summary>
    /// 危險
    /// </summary>
    Dangerous = 4
}

/// <summary>
/// 緊急程度
/// </summary>
public enum UrgencyLevel
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