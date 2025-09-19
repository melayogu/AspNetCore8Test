using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 植物模型
/// </summary>
public class Plant
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string ScientificName { get; set; } = string.Empty;

    [Required]
    public PlantType Type { get; set; }

    [Required]
    public PlantStatus Status { get; set; }

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
    /// 種植日期
    /// </summary>
    public DateTime PlantingDate { get; set; }

    /// <summary>
    /// 預估高度(公分)
    /// </summary>
    public int? EstimatedHeight { get; set; }

    /// <summary>
    /// 預估直徑(公分)
    /// </summary>
    public int? EstimatedDiameter { get; set; }

    /// <summary>
    /// 澆水頻率(天)
    /// </summary>
    public int WateringFrequency { get; set; }

    /// <summary>
    /// 施肥頻率(天)
    /// </summary>
    public int FertilizingFrequency { get; set; }

    /// <summary>
    /// 最後澆水日期
    /// </summary>
    public DateTime? LastWateringDate { get; set; }

    /// <summary>
    /// 最後施肥日期
    /// </summary>
    public DateTime? LastFertilizingDate { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;

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
    /// 養護記錄
    /// </summary>
    public virtual ICollection<PlantCareRecord> CareRecords { get; set; } = new List<PlantCareRecord>();

    /// <summary>
    /// 疾病記錄
    /// </summary>
    public virtual ICollection<PlantDiseaseRecord> DiseaseRecords { get; set; } = new List<PlantDiseaseRecord>();
}

/// <summary>
/// 植物類型
/// </summary>
public enum PlantType
{
    /// <summary>
    /// 樹木
    /// </summary>
    Tree = 1,

    /// <summary>
    /// 灌木
    /// </summary>
    Shrub = 2,

    /// <summary>
    /// 花卉
    /// </summary>
    Flower = 3,

    /// <summary>
    /// 草本植物
    /// </summary>
    Herb = 4,

    /// <summary>
    /// 藤本植物
    /// </summary>
    Vine = 5,

    /// <summary>
    /// 水生植物
    /// </summary>
    Aquatic = 6
}

/// <summary>
/// 植物狀態
/// </summary>
public enum PlantStatus
{
    /// <summary>
    /// 健康
    /// </summary>
    Healthy = 1,

    /// <summary>
    /// 需要關注
    /// </summary>
    NeedsAttention = 2,

    /// <summary>
    /// 生病
    /// </summary>
    Sick = 3,

    /// <summary>
    /// 死亡
    /// </summary>
    Dead = 4,

    /// <summary>
    /// 已移除
    /// </summary>
    Removed = 5
}