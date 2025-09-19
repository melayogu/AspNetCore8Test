using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 植物創建 DTO
/// </summary>
public class CreatePlantDto
{
    [Required(ErrorMessage = "植物名稱為必填")]
    [StringLength(100, ErrorMessage = "植物名稱不得超過100個字元")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "學名為必填")]
    [StringLength(100, ErrorMessage = "學名不得超過100個字元")]
    public string ScientificName { get; set; } = string.Empty;

    [Required(ErrorMessage = "植物類型為必填")]
    public int Type { get; set; }

    [Required(ErrorMessage = "植物狀態為必填")]
    public int Status { get; set; }

    [StringLength(200, ErrorMessage = "位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
    public decimal? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
    public decimal? Longitude { get; set; }

    [Required(ErrorMessage = "種植日期為必填")]
    public DateTime PlantingDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "預估高度必須大於0")]
    public int? EstimatedHeight { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "預估直徑必須大於0")]
    public int? EstimatedDiameter { get; set; }

    [Range(1, 365, ErrorMessage = "澆水頻率必須在1-365天之間")]
    public int WateringFrequency { get; set; } = 7;

    [Range(1, 365, ErrorMessage = "施肥頻率必須在1-365天之間")]
    public int FertilizingFrequency { get; set; } = 30;

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 植物更新 DTO
/// </summary>
public class UpdatePlantDto
{
    [Required(ErrorMessage = "植物名稱為必填")]
    [StringLength(100, ErrorMessage = "植物名稱不得超過100個字元")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "學名為必填")]
    [StringLength(100, ErrorMessage = "學名不得超過100個字元")]
    public string ScientificName { get; set; } = string.Empty;

    [Required(ErrorMessage = "植物類型為必填")]
    public int Type { get; set; }

    [Required(ErrorMessage = "植物狀態為必填")]
    public int Status { get; set; }

    [StringLength(200, ErrorMessage = "位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
    public decimal? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
    public decimal? Longitude { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "預估高度必須大於0")]
    public int? EstimatedHeight { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "預估直徑必須大於0")]
    public int? EstimatedDiameter { get; set; }

    [Range(1, 365, ErrorMessage = "澆水頻率必須在1-365天之間")]
    public int WateringFrequency { get; set; }

    [Range(1, 365, ErrorMessage = "施肥頻率必須在1-365天之間")]
    public int FertilizingFrequency { get; set; }

    public DateTime? LastWateringDate { get; set; }

    public DateTime? LastFertilizingDate { get; set; }

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// 植物查詢 DTO
/// </summary>
public class PlantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime PlantingDate { get; set; }
    public int? EstimatedHeight { get; set; }
    public int? EstimatedDiameter { get; set; }
    public int WateringFrequency { get; set; }
    public int FertilizingFrequency { get; set; }
    public DateTime? LastWateringDate { get; set; }
    public DateTime? LastFertilizingDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// 下次澆水日期
    /// </summary>
    public DateTime? NextWateringDate => LastWateringDate?.AddDays(WateringFrequency);
    
    /// <summary>
    /// 下次施肥日期
    /// </summary>
    public DateTime? NextFertilizingDate => LastFertilizingDate?.AddDays(FertilizingFrequency);
}

/// <summary>
/// 植物養護記錄創建 DTO
/// </summary>
public class CreatePlantCareRecordDto
{
    [Required(ErrorMessage = "植物ID為必填")]
    public int PlantId { get; set; }

    [Required(ErrorMessage = "養護類型為必填")]
    public int CareType { get; set; }

    [Required(ErrorMessage = "養護日期為必填")]
    public DateTime CareDate { get; set; }

    [Required(ErrorMessage = "養護人員姓名為必填")]
    [StringLength(100, ErrorMessage = "養護人員姓名不得超過100個字元")]
    public string CaregiverName { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "描述不得超過1000個字元")]
    public string Description { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "材料工具不得超過500個字元")]
    public string MaterialsUsed { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "成本不得為負數")]
    public decimal Cost { get; set; }

    public DateTime? NextScheduledDate { get; set; }

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 植物養護記錄查詢 DTO
/// </summary>
public class PlantCareRecordDto
{
    public int Id { get; set; }
    public int PlantId { get; set; }
    public string PlantName { get; set; } = string.Empty;
    public string CareType { get; set; } = string.Empty;
    public DateTime CareDate { get; set; }
    public string CaregiverName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MaterialsUsed { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public DateTime? NextScheduledDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}