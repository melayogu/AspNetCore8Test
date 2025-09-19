using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 植物養護記錄模型
/// </summary>
public class PlantCareRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Plant")]
    public int PlantId { get; set; }

    [Required]
    public CareType CareType { get; set; }

    [Required]
    public DateTime CareDate { get; set; }

    [Required]
    [StringLength(100)]
    public string CaregiverName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 使用的材料或工具
    /// </summary>
    [StringLength(500)]
    public string MaterialsUsed { get; set; } = string.Empty;

    /// <summary>
    /// 成本
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// 下次預定養護日期
    /// </summary>
    public DateTime? NextScheduledDate { get; set; }

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
    /// 關聯的植物
    /// </summary>
    public virtual Plant Plant { get; set; } = null!;
}

/// <summary>
/// 養護類型
/// </summary>
public enum CareType
{
    /// <summary>
    /// 澆水
    /// </summary>
    Watering = 1,

    /// <summary>
    /// 施肥
    /// </summary>
    Fertilizing = 2,

    /// <summary>
    /// 修剪
    /// </summary>
    Pruning = 3,

    /// <summary>
    /// 除草
    /// </summary>
    Weeding = 4,

    /// <summary>
    /// 病蟲害防治
    /// </summary>
    PestControl = 5,

    /// <summary>
    /// 土壤改良
    /// </summary>
    SoilImprovement = 6,

    /// <summary>
    /// 支撐固定
    /// </summary>
    Support = 7,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 8
}