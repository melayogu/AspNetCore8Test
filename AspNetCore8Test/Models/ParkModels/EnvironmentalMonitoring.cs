using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.ParkModels;

/// <summary>
/// 環境監測記錄模型
/// </summary>
public class EnvironmentalMonitoring
{
    [Key]
    public int Id { get; set; }

    [Required]
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

    [Required]
    public DateTime RecordedAt { get; set; }

    /// <summary>
    /// 空氣品質指數 (AQI)
    /// </summary>
    public int? AirQualityIndex { get; set; }

    /// <summary>
    /// PM2.5 濃度 (μg/m³)
    /// </summary>
    public decimal? PM25 { get; set; }

    /// <summary>
    /// PM10 濃度 (μg/m³)
    /// </summary>
    public decimal? PM10 { get; set; }

    /// <summary>
    /// 二氧化碳濃度 (ppm)
    /// </summary>
    public decimal? CO2Level { get; set; }

    /// <summary>
    /// 溫度 (°C)
    /// </summary>
    public decimal? Temperature { get; set; }

    /// <summary>
    /// 濕度 (%)
    /// </summary>
    public decimal? Humidity { get; set; }

    /// <summary>
    /// 噪音等級 (dB)
    /// </summary>
    public decimal? NoiseLevel { get; set; }

    /// <summary>
    /// 水質 pH 值
    /// </summary>
    public decimal? WaterPH { get; set; }

    /// <summary>
    /// 水中溶氧量 (mg/L)
    /// </summary>
    public decimal? DissolvedOxygen { get; set; }

    /// <summary>
    /// 水溫 (°C)
    /// </summary>
    public decimal? WaterTemperature { get; set; }

    /// <summary>
    /// 水質濁度 (NTU)
    /// </summary>
    public decimal? WaterTurbidity { get; set; }

    /// <summary>
    /// 紫外線指數
    /// </summary>
    public decimal? UVIndex { get; set; }

    /// <summary>
    /// 風速 (m/s)
    /// </summary>
    public decimal? WindSpeed { get; set; }

    /// <summary>
    /// 風向 (度)
    /// </summary>
    public decimal? WindDirection { get; set; }

    /// <summary>
    /// 降雨量 (mm)
    /// </summary>
    public decimal? Rainfall { get; set; }

    /// <summary>
    /// 監測設備ID
    /// </summary>
    [StringLength(100)]
    public string DeviceId { get; set; } = string.Empty;

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
    /// 警報記錄
    /// </summary>
    public virtual ICollection<EnvironmentalAlert> Alerts { get; set; } = new List<EnvironmentalAlert>();
}