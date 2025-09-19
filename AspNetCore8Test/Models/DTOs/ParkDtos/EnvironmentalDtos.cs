using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.ParkDtos;

/// <summary>
/// 環境監測記錄創建 DTO
/// </summary>
public class CreateEnvironmentalMonitoringDto
{
    [Required(ErrorMessage = "位置為必填")]
    [StringLength(200, ErrorMessage = "位置不得超過200個字元")]
    public string Location { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
    public decimal? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
    public decimal? Longitude { get; set; }

    [Required(ErrorMessage = "記錄時間為必填")]
    public DateTime RecordedAt { get; set; }

    [Range(0, 500, ErrorMessage = "空氣品質指數必須在0-500之間")]
    public int? AirQualityIndex { get; set; }

    [Range(0, 1000, ErrorMessage = "PM2.5濃度必須在0-1000之間")]
    public decimal? PM25 { get; set; }

    [Range(0, 1000, ErrorMessage = "PM10濃度必須在0-1000之間")]
    public decimal? PM10 { get; set; }

    [Range(0, 50000, ErrorMessage = "CO2濃度必須在0-50000之間")]
    public decimal? CO2Level { get; set; }

    [Range(-50, 80, ErrorMessage = "溫度必須在-50到80之間")]
    public decimal? Temperature { get; set; }

    [Range(0, 100, ErrorMessage = "濕度必須在0-100之間")]
    public decimal? Humidity { get; set; }

    [Range(0, 200, ErrorMessage = "噪音等級必須在0-200之間")]
    public decimal? NoiseLevel { get; set; }

    [Range(0, 14, ErrorMessage = "pH值必須在0-14之間")]
    public decimal? WaterPH { get; set; }

    [Range(0, 50, ErrorMessage = "溶氧量必須在0-50之間")]
    public decimal? DissolvedOxygen { get; set; }

    [Range(0, 50, ErrorMessage = "水溫必須在0-50之間")]
    public decimal? WaterTemperature { get; set; }

    [Range(0, 1000, ErrorMessage = "濁度必須在0-1000之間")]
    public decimal? WaterTurbidity { get; set; }

    [Range(0, 20, ErrorMessage = "紫外線指數必須在0-20之間")]
    public decimal? UVIndex { get; set; }

    [Range(0, 200, ErrorMessage = "風速必須在0-200之間")]
    public decimal? WindSpeed { get; set; }

    [Range(0, 360, ErrorMessage = "風向必須在0-360之間")]
    public decimal? WindDirection { get; set; }

    [Range(0, 1000, ErrorMessage = "降雨量必須在0-1000之間")]
    public decimal? Rainfall { get; set; }

    [StringLength(100, ErrorMessage = "設備ID不得超過100個字元")]
    public string DeviceId { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "備註不得超過1000個字元")]
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// 環境監測記錄查詢 DTO
/// </summary>
public class EnvironmentalMonitoringDto
{
    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime RecordedAt { get; set; }
    public int? AirQualityIndex { get; set; }
    public decimal? PM25 { get; set; }
    public decimal? PM10 { get; set; }
    public decimal? CO2Level { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Humidity { get; set; }
    public decimal? NoiseLevel { get; set; }
    public decimal? WaterPH { get; set; }
    public decimal? DissolvedOxygen { get; set; }
    public decimal? WaterTemperature { get; set; }
    public decimal? WaterTurbidity { get; set; }
    public decimal? UVIndex { get; set; }
    public decimal? WindSpeed { get; set; }
    public decimal? WindDirection { get; set; }
    public decimal? Rainfall { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 空氣品質等級
    /// </summary>
    public string AirQualityLevel 
    { 
        get 
        {
            return AirQualityIndex switch
            {
                null => "無數據",
                <= 50 => "良好",
                <= 100 => "普通",
                <= 150 => "對敏感族群不健康",
                <= 200 => "對所有族群不健康",
                <= 300 => "非常不健康",
                _ => "危險"
            };
        }
    }
    
    /// <summary>
    /// 溫度舒適度
    /// </summary>
    public string TemperatureComfort
    {
        get
        {
            return Temperature switch
            {
                null => "無數據",
                < 10 => "寒冷",
                >= 10 and < 16 => "涼爽",
                >= 16 and < 24 => "舒適",
                >= 24 and < 28 => "溫暖",
                >= 28 and < 35 => "炎熱",
                _ => "酷熱"
            };
        }
    }
}

/// <summary>
/// 環境警報創建 DTO
/// </summary>
public class CreateEnvironmentalAlertDto
{
    [Required(ErrorMessage = "監測ID為必填")]
    public int MonitoringId { get; set; }

    [Required(ErrorMessage = "警報類型為必填")]
    public int AlertType { get; set; }

    [Required(ErrorMessage = "警報等級為必填")]
    public int AlertLevel { get; set; }

    [Required(ErrorMessage = "警報訊息為必填")]
    [StringLength(200, ErrorMessage = "警報訊息不得超過200個字元")]
    public string Message { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "描述不得超過1000個字元")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "觸發時間為必填")]
    public DateTime TriggeredAt { get; set; }
}

/// <summary>
/// 環境警報查詢 DTO
/// </summary>
public class EnvironmentalAlertDto
{
    public int Id { get; set; }
    public int MonitoringId { get; set; }
    public string Location { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string AlertLevel { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TriggeredAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public bool IsResolved { get; set; }
    public string ResolvedBy { get; set; } = string.Empty;
    public string ResolvedNotes { get; set; } = string.Empty;
    public bool IsNotified { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 環境統計 DTO
/// </summary>
public class EnvironmentalStatsDto
{
    public int TotalRecords { get; set; }
    public int ActiveAlerts { get; set; }
    public decimal? LatestTemperature { get; set; }
    public decimal? LatestHumidity { get; set; }
    public int? LatestAQI { get; set; }
    public string AirQualityStatus { get; set; } = string.Empty;
    public DateTime? LastUpdated { get; set; }
    
    public EnvironmentalTrend TemperatureTrend { get; set; } = new();
    public EnvironmentalTrend HumidityTrend { get; set; } = new();
    public EnvironmentalTrend AQITrend { get; set; } = new();
}

/// <summary>
/// 環境數據趨勢
/// </summary>
public class EnvironmentalTrend
{
    public decimal? Average { get; set; }
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }
    public string TrendDirection { get; set; } = string.Empty; // "上升", "下降", "穩定"
}