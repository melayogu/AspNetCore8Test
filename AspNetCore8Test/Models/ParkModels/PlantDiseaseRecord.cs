using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 植物疾病記錄模型
/// </summary>
public class PlantDiseaseRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Plant")]
    public int PlantId { get; set; }

    [Required]
    [StringLength(100)]
    public string DiseaseName { get; set; } = string.Empty;

    [Required]
    public DiseaseType DiseaseType { get; set; }

    [Required]
    public DiseaseSeverity Severity { get; set; }

    [Required]
    public DateTime DetectedDate { get; set; }

    [StringLength(100)]
    public string DetectedBy { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Symptoms { get; set; } = string.Empty;

    [StringLength(1000)]
    public string TreatmentApplied { get; set; } = string.Empty;

    /// <summary>
    /// 治療開始日期
    /// </summary>
    public DateTime? TreatmentStartDate { get; set; }

    /// <summary>
    /// 治療結束日期
    /// </summary>
    public DateTime? TreatmentEndDate { get; set; }

    [Required]
    public TreatmentStatus TreatmentStatus { get; set; }

    /// <summary>
    /// 治療成本
    /// </summary>
    public decimal TreatmentCost { get; set; }

    /// <summary>
    /// 是否傳染性
    /// </summary>
    public bool IsContagious { get; set; }

    /// <summary>
    /// 照片路徑
    /// </summary>
    [StringLength(500)]
    public string PhotoPath { get; set; } = string.Empty;

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
    /// 關聯的植物
    /// </summary>
    public virtual Plant Plant { get; set; } = null!;
}

/// <summary>
/// 疾病類型
/// </summary>
public enum DiseaseType
{
    /// <summary>
    /// 真菌感染
    /// </summary>
    Fungal = 1,

    /// <summary>
    /// 細菌感染
    /// </summary>
    Bacterial = 2,

    /// <summary>
    /// 病毒感染
    /// </summary>
    Viral = 3,

    /// <summary>
    /// 蟲害
    /// </summary>
    Pest = 4,

    /// <summary>
    /// 營養不良
    /// </summary>
    Nutritional = 5,

    /// <summary>
    /// 環境因素
    /// </summary>
    Environmental = 6,

    /// <summary>
    /// 其他
    /// </summary>
    Other = 7
}

/// <summary>
/// 疾病嚴重程度
/// </summary>
public enum DiseaseSeverity
{
    /// <summary>
    /// 輕微
    /// </summary>
    Mild = 1,

    /// <summary>
    /// 中等
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// 嚴重
    /// </summary>
    Severe = 3,

    /// <summary>
    /// 致命
    /// </summary>
    Critical = 4
}

/// <summary>
/// 治療狀態
/// </summary>
public enum TreatmentStatus
{
    /// <summary>
    /// 未治療
    /// </summary>
    NotTreated = 1,

    /// <summary>
    /// 治療中
    /// </summary>
    UnderTreatment = 2,

    /// <summary>
    /// 治療完成
    /// </summary>
    TreatmentCompleted = 3,

    /// <summary>
    /// 已康復
    /// </summary>
    Recovered = 4,

    /// <summary>
    /// 無法治療
    /// </summary>
    Untreatable = 5
}