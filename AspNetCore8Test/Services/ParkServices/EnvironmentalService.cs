using AspNetCore8Test.Models.ParkModels;
using AspNetCore8Test.Models.DTOs.ParkDtos;

namespace AspNetCore8Test.Services.ParkServices;

/// <summary>
/// 環境監測服務介面
/// </summary>
public interface IEnvironmentalService
{
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetAllAsync();
    Task<EnvironmentalMonitoringDto?> GetByIdAsync(int id);
    Task<EnvironmentalMonitoringDto> CreateAsync(CreateEnvironmentalMonitoringDto dto);
    Task<bool> UpdateAsync(int id, CreateEnvironmentalMonitoringDto dto);
    Task<bool> DeleteAsync(int id);
    
    // 警報相關
    Task<IEnumerable<EnvironmentalAlertDto>> GetAlertsAsync(bool includeResolved = false);
    Task<EnvironmentalAlertDto> CreateAlertAsync(CreateEnvironmentalAlertDto dto);
    Task<bool> ResolveAlertAsync(int alertId, string resolvedBy, string notes);
    
    // 統計和查詢
    Task<EnvironmentalStatsDto> GetStatsAsync();
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetByLocationAsync(string location);
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<EnvironmentalMonitoringDto>> SearchAsync(string searchTerm);
    
    // 特殊查詢
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetLatestByLocationAsync();
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetPoorAirQualityAsync();
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetExtremeTemperatureAsync();
    Task<IEnumerable<EnvironmentalMonitoringDto>> GetHighNoiseAsync();
}

/// <summary>
/// 環境監測服務實作
/// </summary>
public class EnvironmentalService : IEnvironmentalService
{
    private static readonly List<EnvironmentalMonitoring> _monitoring = new();
    private static readonly List<AspNetCore8Test.Models.ParkModels.EnvironmentalAlert> _alerts = new();
    private static int _monitoringIdCounter = 1;
    private static int _alertIdCounter = 1;
    private static readonly object _lock = new();
    
    static EnvironmentalService()
    {
        InitializeSampleData();
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetAllAsync()
    {
        await Task.Delay(10); // 模擬異步操作
        
        lock (_lock)
        {
            return _monitoring
                .OrderByDescending(m => m.RecordedAt)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<EnvironmentalMonitoringDto?> GetByIdAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var monitoring = _monitoring.FirstOrDefault(m => m.Id == id);
            return monitoring != null ? MapToDto(monitoring) : null;
        }
    }
    
    public async Task<EnvironmentalMonitoringDto> CreateAsync(CreateEnvironmentalMonitoringDto dto)
    {
        await Task.Delay(10);
        
        var monitoring = new EnvironmentalMonitoring
        {
            Id = Interlocked.Increment(ref _monitoringIdCounter),
            Location = dto.Location,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            RecordedAt = dto.RecordedAt,
            AirQualityIndex = dto.AirQualityIndex,
            PM25 = dto.PM25,
            PM10 = dto.PM10,
            CO2Level = dto.CO2Level,
            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            NoiseLevel = dto.NoiseLevel,
            WaterPH = dto.WaterPH,
            DissolvedOxygen = dto.DissolvedOxygen,
            WaterTemperature = dto.WaterTemperature,
            WaterTurbidity = dto.WaterTurbidity,
            UVIndex = dto.UVIndex,
            WindSpeed = dto.WindSpeed,
            WindDirection = dto.WindDirection,
            Rainfall = dto.Rainfall,
            DeviceId = dto.DeviceId,
            Notes = dto.Notes,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _monitoring.Add(monitoring);
        }
        
        // 檢查是否需要創建警報
        await CheckAndCreateAlertsAsync(monitoring);
        
        return MapToDto(monitoring);
    }
    
    public async Task<bool> UpdateAsync(int id, CreateEnvironmentalMonitoringDto dto)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var monitoring = _monitoring.FirstOrDefault(m => m.Id == id);
            if (monitoring == null) return false;
            
            monitoring.Location = dto.Location;
            monitoring.Latitude = dto.Latitude;
            monitoring.Longitude = dto.Longitude;
            monitoring.RecordedAt = dto.RecordedAt;
            monitoring.AirQualityIndex = dto.AirQualityIndex;
            monitoring.PM25 = dto.PM25;
            monitoring.PM10 = dto.PM10;
            monitoring.CO2Level = dto.CO2Level;
            monitoring.Temperature = dto.Temperature;
            monitoring.Humidity = dto.Humidity;
            monitoring.NoiseLevel = dto.NoiseLevel;
            monitoring.WaterPH = dto.WaterPH;
            monitoring.DissolvedOxygen = dto.DissolvedOxygen;
            monitoring.WaterTemperature = dto.WaterTemperature;
            monitoring.WaterTurbidity = dto.WaterTurbidity;
            monitoring.UVIndex = dto.UVIndex;
            monitoring.WindSpeed = dto.WindSpeed;
            monitoring.WindDirection = dto.WindDirection;
            monitoring.Rainfall = dto.Rainfall;
            monitoring.DeviceId = dto.DeviceId;
            monitoring.Notes = dto.Notes;
            
            return true;
        }
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var monitoring = _monitoring.FirstOrDefault(m => m.Id == id);
            if (monitoring == null) return false;
            
            _monitoring.Remove(monitoring);
            return true;
        }
    }
    
    public async Task<IEnumerable<EnvironmentalAlertDto>> GetAlertsAsync(bool includeResolved = false)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var query = _alerts.AsQueryable();
            
            if (!includeResolved)
            {
                query = query.Where(a => !a.IsResolved);
            }
            
            return query
                .OrderByDescending(a => a.TriggeredAt)
                .Select(MapAlertToDto)
                .ToList();
        }
    }
    
    public async Task<EnvironmentalAlertDto> CreateAlertAsync(CreateEnvironmentalAlertDto dto)
    {
        await Task.Delay(10);
        
        var monitoring = _monitoring.FirstOrDefault(m => m.Id == dto.MonitoringId);
        if (monitoring == null)
            throw new InvalidOperationException("找不到對應的監測記錄");
        
        var alert = new AspNetCore8Test.Models.ParkModels.EnvironmentalAlert
        {
            Id = Interlocked.Increment(ref _alertIdCounter),
            MonitoringId = dto.MonitoringId,
            AlertType = (AlertType)dto.AlertType,
            AlertLevel = (AlertLevel)dto.AlertLevel,
            Message = dto.Message,
            Description = dto.Description,
            TriggeredAt = dto.TriggeredAt,
            CreatedAt = DateTime.Now
        };
        
        lock (_lock)
        {
            _alerts.Add(alert);
        }
        
        return MapAlertToDto(alert);
    }
    
    public async Task<bool> ResolveAlertAsync(int alertId, string resolvedBy, string notes)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null) return false;
            
            alert.IsResolved = true;
            alert.ResolvedAt = DateTime.Now;
            alert.ResolvedBy = resolvedBy;
            alert.ResolvedNotes = notes;
            
            return true;
        }
    }
    
    public async Task<EnvironmentalStatsDto> GetStatsAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            var latestRecord = _monitoring
                .OrderByDescending(m => m.RecordedAt)
                .FirstOrDefault();
            
            var recentRecords = _monitoring
                .Where(m => m.RecordedAt >= DateTime.Now.AddDays(-7))
                .ToList();
            
            var activeAlerts = _alerts.Count(a => !a.IsResolved);
            
            return new EnvironmentalStatsDto
            {
                TotalRecords = _monitoring.Count,
                ActiveAlerts = activeAlerts,
                LatestTemperature = latestRecord?.Temperature,
                LatestHumidity = latestRecord?.Humidity,
                LatestAQI = latestRecord?.AirQualityIndex,
                AirQualityStatus = GetAirQualityStatus(latestRecord?.AirQualityIndex),
                LastUpdated = latestRecord?.RecordedAt,
                TemperatureTrend = CalculateTrend(recentRecords, r => r.Temperature),
                HumidityTrend = CalculateTrend(recentRecords, r => r.Humidity),
                AQITrend = CalculateTrend(recentRecords, r => r.AirQualityIndex)
            };
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetByLocationAsync(string location)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => m.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.RecordedAt)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => m.RecordedAt >= startDate && m.RecordedAt <= endDate)
                .OrderByDescending(m => m.RecordedAt)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> SearchAsync(string searchTerm)
    {
        await Task.Delay(10);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => 
                    m.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    m.DeviceId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (m.Notes != null && m.Notes.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(m => m.RecordedAt)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetLatestByLocationAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .GroupBy(m => m.Location)
                .Select(g => g.OrderByDescending(m => m.RecordedAt).First())
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetPoorAirQualityAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => m.AirQualityIndex.HasValue && m.AirQualityIndex > 100)
                .OrderByDescending(m => m.AirQualityIndex)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetExtremeTemperatureAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => m.Temperature.HasValue && (m.Temperature < 10 || m.Temperature > 35))
                .OrderByDescending(m => m.RecordedAt)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    public async Task<IEnumerable<EnvironmentalMonitoringDto>> GetHighNoiseAsync()
    {
        await Task.Delay(10);
        
        lock (_lock)
        {
            return _monitoring
                .Where(m => m.NoiseLevel.HasValue && m.NoiseLevel > 70)
                .OrderByDescending(m => m.NoiseLevel)
                .Select(MapToDto)
                .ToList();
        }
    }
    
    private async Task CheckAndCreateAlertsAsync(EnvironmentalMonitoring monitoring)
    {
        var alerts = new List<CreateEnvironmentalAlertDto>();
        
        // 空氣品質警報
        if (monitoring.AirQualityIndex.HasValue && monitoring.AirQualityIndex > 150)
        {
            alerts.Add(new CreateEnvironmentalAlertDto
            {
                MonitoringId = monitoring.Id,
                AlertType = (int)AlertType.AirQuality,
                AlertLevel = monitoring.AirQualityIndex > 200 ? (int)AlertLevel.Danger : (int)AlertLevel.Warning,
                Message = $"空氣品質不良警報 - AQI: {monitoring.AirQualityIndex}",
                Description = "空氣品質指數超過標準值，建議減少戶外活動",
                TriggeredAt = monitoring.RecordedAt
            });
        }
        
        // 溫度警報
        if (monitoring.Temperature.HasValue && (monitoring.Temperature < 5 || monitoring.Temperature > 38))
        {
            alerts.Add(new CreateEnvironmentalAlertDto
            {
                MonitoringId = monitoring.Id,
                AlertType = (int)AlertType.Temperature,
                AlertLevel = (int)AlertLevel.Warning,
                Message = $"極端溫度警報 - {monitoring.Temperature}°C",
                Description = monitoring.Temperature < 5 ? "低溫警報，注意保暖" : "高溫警報，注意防暑",
                TriggeredAt = monitoring.RecordedAt
            });
        }
        
        // 噪音警報
        if (monitoring.NoiseLevel.HasValue && monitoring.NoiseLevel > 80)
        {
            alerts.Add(new CreateEnvironmentalAlertDto
            {
                MonitoringId = monitoring.Id,
                AlertType = (int)AlertType.Noise,
                AlertLevel = monitoring.NoiseLevel > 90 ? (int)AlertLevel.Danger : (int)AlertLevel.Warning,
                Message = $"噪音超標警報 - {monitoring.NoiseLevel} dB",
                Description = "噪音等級超過舒適標準",
                TriggeredAt = monitoring.RecordedAt
            });
        }
        
        // 創建警報
        foreach (var alertDto in alerts)
        {
            await CreateAlertAsync(alertDto);
        }
    }
    
    private static EnvironmentalMonitoringDto MapToDto(EnvironmentalMonitoring monitoring)
    {
        return new EnvironmentalMonitoringDto
        {
            Id = monitoring.Id,
            Location = monitoring.Location,
            Latitude = monitoring.Latitude,
            Longitude = monitoring.Longitude,
            RecordedAt = monitoring.RecordedAt,
            AirQualityIndex = monitoring.AirQualityIndex,
            PM25 = monitoring.PM25,
            PM10 = monitoring.PM10,
            CO2Level = monitoring.CO2Level,
            Temperature = monitoring.Temperature,
            Humidity = monitoring.Humidity,
            NoiseLevel = monitoring.NoiseLevel,
            WaterPH = monitoring.WaterPH,
            DissolvedOxygen = monitoring.DissolvedOxygen,
            WaterTemperature = monitoring.WaterTemperature,
            WaterTurbidity = monitoring.WaterTurbidity,
            UVIndex = monitoring.UVIndex,
            WindSpeed = monitoring.WindSpeed,
            WindDirection = monitoring.WindDirection,
            Rainfall = monitoring.Rainfall,
            DeviceId = monitoring.DeviceId,
            Notes = monitoring.Notes,
            CreatedAt = monitoring.CreatedAt
        };
    }
    
    private static EnvironmentalAlertDto MapAlertToDto(AspNetCore8Test.Models.ParkModels.EnvironmentalAlert alert)
    {
        var monitoring = _monitoring.FirstOrDefault(m => m.Id == alert.MonitoringId);
        
        return new EnvironmentalAlertDto
        {
            Id = alert.Id,
            MonitoringId = alert.MonitoringId,
            Location = monitoring?.Location ?? "",
            AlertType = alert.AlertType.ToString(),
            AlertLevel = alert.AlertLevel.ToString(),
            Message = alert.Message,
            Description = alert.Description,
            TriggeredAt = alert.TriggeredAt,
            ResolvedAt = alert.ResolvedAt,
            IsResolved = alert.IsResolved,
            ResolvedBy = alert.ResolvedBy,
            ResolvedNotes = alert.ResolvedNotes,
            IsNotified = alert.IsNotified,
            CreatedAt = alert.CreatedAt
        };
    }
    
    private static string GetAirQualityStatus(int? aqi)
    {
        return aqi switch
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
    
    private static EnvironmentalTrend CalculateTrend(List<EnvironmentalMonitoring> records, Func<EnvironmentalMonitoring, decimal?> selector)
    {
        var values = records.Select(selector).Where(v => v.HasValue).Select(v => v!.Value).ToList();
        
        if (!values.Any())
        {
            return new EnvironmentalTrend { TrendDirection = "無數據" };
        }
        
        var average = values.Average();
        var min = values.Min();
        var max = values.Max();
        
        // 簡單的趨勢計算：比較前半部分和後半部分的平均值
        var midIndex = values.Count / 2;
        var firstHalf = values.Take(midIndex).Average();
        var secondHalf = values.Skip(midIndex).Average();
        
        var trendDirection = "穩定";
        if (Math.Abs(secondHalf - firstHalf) >= average * 0.1m)
        {
            trendDirection = secondHalf > firstHalf ? "上升" : "下降";
        }
        
        return new EnvironmentalTrend
        {
            Average = average,
            Min = min,
            Max = max,
            TrendDirection = trendDirection
        };
    }
    
    private static void InitializeSampleData()
    {
        var locations = new[] { "湖心亭", "東岸步道", "西側涼亭", "北入口", "南側停車場", "兒童遊戲區" };
        var random = new Random();
        
        for (int i = 0; i < 50; i++)
        {
            var location = locations[random.Next(locations.Length)];
            var recordedAt = DateTime.Now.AddDays(-random.Next(30)).AddHours(-random.Next(24));
            
            _monitoring.Add(new EnvironmentalMonitoring
            {
                Id = Interlocked.Increment(ref _monitoringIdCounter),
                Location = location,
                Latitude = 25.082m + (decimal)(random.NextDouble() - 0.5) * 0.01m,
                Longitude = 121.563m + (decimal)(random.NextDouble() - 0.5) * 0.01m,
                RecordedAt = recordedAt,
                AirQualityIndex = random.Next(30, 180),
                PM25 = (decimal)(random.NextDouble() * 80 + 10),
                PM10 = (decimal)(random.NextDouble() * 120 + 20),
                CO2Level = (decimal)(random.NextDouble() * 800 + 400),
                Temperature = (decimal)(random.NextDouble() * 20 + 15),
                Humidity = (decimal)(random.NextDouble() * 40 + 40),
                NoiseLevel = (decimal)(random.NextDouble() * 40 + 40),
                WaterPH = (decimal)(random.NextDouble() * 3 + 6),
                DissolvedOxygen = (decimal)(random.NextDouble() * 8 + 4),
                WaterTemperature = (decimal)(random.NextDouble() * 10 + 18),
                WaterTurbidity = (decimal)(random.NextDouble() * 20 + 2),
                UVIndex = (decimal)(random.NextDouble() * 12 + 2),
                WindSpeed = (decimal)(random.NextDouble() * 15 + 2),
                WindDirection = (decimal)(random.NextDouble() * 360),
                Rainfall = recordedAt.Hour % 4 == 0 ? (decimal)(random.NextDouble() * 20) : 0,
                DeviceId = $"ENV-{location.Substring(0, 2)}-{random.Next(100, 999)}",
                Notes = i % 5 == 0 ? "定期檢測" : string.Empty,
                CreatedAt = recordedAt
            });
        }
        
        // 添加一些警報記錄
        var highAQIRecords = _monitoring.Where(m => m.AirQualityIndex > 120).Take(3);
        foreach (var record in highAQIRecords)
        {
            var alertId = Interlocked.Increment(ref _alertIdCounter);
            _alerts.Add(new AspNetCore8Test.Models.ParkModels.EnvironmentalAlert
            {
                Id = alertId,
                MonitoringId = record.Id,
                AlertType = AlertType.AirQuality,
                AlertLevel = AlertLevel.Warning,
                Message = $"空氣品質不良 - AQI: {record.AirQualityIndex}",
                Description = "空氣品質指數超過標準值",
                TriggeredAt = record.RecordedAt,
                IsResolved = alertId % 2 == 0,
                ResolvedAt = alertId % 2 == 0 ? record.RecordedAt.AddHours(2) : null,
                ResolvedBy = alertId % 2 == 0 ? "系統管理員" : string.Empty,
                CreatedAt = record.RecordedAt
            });
        }
    }
}