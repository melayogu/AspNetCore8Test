using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 設施創建 DTO
/// </summary>
public class CreateFacilityDto
{
    [Required(ErrorMessage = "設施名稱為必填")]
    [StringLength(100, ErrorMessage = "設施名稱不得超過100個字元")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "描述不得超過500個字元")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "設施類型為必填")]
    public int Type { get; set; }

    [Required(ErrorMessage = "設施狀態為必填")]
    public int Status { get; set; }

    [StringLength(200, ErrorMessage = "位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
    public decimal? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
    public decimal? Longitude { get; set; }

    [Required(ErrorMessage = "安裝日期為必填")]
    public DateTime InstallationDate { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "維護成本不得為負數")]
    public decimal MaintenanceCost { get; set; }
}

/// <summary>
/// 設施更新 DTO
/// </summary>
public class UpdateFacilityDto
{
    [Required(ErrorMessage = "設施名稱為必填")]
    [StringLength(100, ErrorMessage = "設施名稱不得超過100個字元")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "描述不得超過500個字元")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "設施類型為必填")]
    public int Type { get; set; }

    [Required(ErrorMessage = "設施狀態為必填")]
    public int Status { get; set; }

    [StringLength(200, ErrorMessage = "位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
    public decimal? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
    public decimal? Longitude { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "維護成本不得為負數")]
    public decimal MaintenanceCost { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// 設施查詢 DTO
/// </summary>
public class FacilityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public decimal MaintenanceCost { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 維護記錄創建 DTO
/// </summary>
public class CreateMaintenanceRecordDto
{
    [Required(ErrorMessage = "設施ID為必填")]
    public int FacilityId { get; set; }

    [Required(ErrorMessage = "維護類型為必填")]
    [StringLength(100, ErrorMessage = "維護類型不得超過100個字元")]
    public string MaintenanceType { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "描述不得超過1000個字元")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "狀態為必填")]
    public int Status { get; set; }

    [Required(ErrorMessage = "預定日期為必填")]
    public DateTime ScheduledDate { get; set; }

    [Required(ErrorMessage = "負責人為必填")]
    [StringLength(100, ErrorMessage = "負責人姓名不得超過100個字元")]
    public string AssignedTo { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "成本不得為負數")]
    public decimal Cost { get; set; }

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 維護記錄查詢 DTO
/// </summary>
public class MaintenanceRecordDto
{
    public int Id { get; set; }
    public int FacilityId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public string MaintenanceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}