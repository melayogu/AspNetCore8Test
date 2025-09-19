using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 公園設施模型
/// </summary>
public class Facility
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public FacilityType Type { get; set; }

    [Required]
    public FacilityStatus Status { get; set; }

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

    /// <summary>
    /// 安裝日期
    /// </summary>
    public DateTime InstallationDate { get; set; }

    /// <summary>
    /// 最後維護日期
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次預定維護日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 維護成本
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 維護記錄
    /// </summary>
    public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();

    /// <summary>
    /// 巡檢記錄
    /// </summary>
    public virtual ICollection<InspectionRecord> InspectionRecords { get; set; } = new List<InspectionRecord>();
}

/// <summary>
/// 設施類型
/// </summary>
public enum FacilityType
{
    /// <summary>
    /// 遊樂設施
    /// </summary>
    Playground = 1,

    /// <summary>
    /// 運動設施
    /// </summary>
    Sports = 2,

    /// <summary>
    /// 休憩設施
    /// </summary>
    Recreation = 3,

    /// <summary>
    /// 照明設備
    /// </summary>
    Lighting = 4,

    /// <summary>
    /// 安全設備
    /// </summary>
    Safety = 5,

    /// <summary>
    /// 景觀設施
    /// </summary>
    Landscape = 6,

    /// <summary>
    /// 服務設施
    /// </summary>
    Service = 7
}

/// <summary>
/// 設施狀態
/// </summary>
public enum FacilityStatus
{
    /// <summary>
    /// 正常使用
    /// </summary>
    Normal = 1,

    /// <summary>
    /// 需要維護
    /// </summary>
    NeedsMaintenance = 2,

    /// <summary>
    /// 維護中
    /// </summary>
    UnderMaintenance = 3,

    /// <summary>
    /// 故障
    /// </summary>
    Broken = 4,

    /// <summary>
    /// 停用
    /// </summary>
    Disabled = 5
}