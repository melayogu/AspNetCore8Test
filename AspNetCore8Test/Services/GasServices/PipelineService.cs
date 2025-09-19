using AspNetCore8Test.Models.GasModels;
using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Services.GasServices
{
    public interface IPipelineService
    {
        Task<IEnumerable<PipelineDto>> GetAllPipelinesAsync();
        Task<PipelineDto?> GetPipelineByIdAsync(int id);
        Task<IEnumerable<PipelineMonitoringDto>> GetPipelineMonitoringDataAsync(int pipelineId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<PipelineAlertDto>> GetPipelineAlertsAsync(bool activeOnly = true);
        Task<IEnumerable<PipelineAlertDto>> GetPipelineAlertsByIdAsync(int pipelineId, bool activeOnly = true);
        Task<PipelineAlertDto> CreateAlertAsync(CreatePipelineAlertDto createAlertDto);
        Task<bool> AcknowledgeAlertAsync(int alertId, string acknowledgedBy);
        Task<bool> ResolveAlertAsync(int alertId, string resolvedBy, string resolution);
        Task<PipelineMonitoringDto> AddMonitoringDataAsync(int pipelineId, decimal pressure, decimal flowRate, decimal temperature, decimal humidity);
        Task<IEnumerable<PipelineDto>> GetPipelinesByStatusAsync(string status);
    }

    public class PipelineService : IPipelineService
    {
        // 模擬資料庫
        private static List<Pipeline> _pipelines = new List<Pipeline>();
        private static List<PipelineMonitoring> _monitoringData = new List<PipelineMonitoring>();
        private static List<PipelineAlert> _alerts = new List<PipelineAlert>();
        private static int _nextPipelineId = 1;
        private static int _nextMonitoringId = 1;
        private static int _nextAlertId = 1;

        public PipelineService()
        {
            // 初始化範例資料
            if (!_pipelines.Any())
            {
                InitializeSampleData();
            }
        }

        public async Task<IEnumerable<PipelineDto>> GetAllPipelinesAsync()
        {
            await Task.Delay(1);
            return _pipelines.Select(MapToPipelineDto);
        }

        public async Task<PipelineDto?> GetPipelineByIdAsync(int id)
        {
            await Task.Delay(1);
            var pipeline = _pipelines.FirstOrDefault(p => p.Id == id);
            return pipeline != null ? MapToPipelineDto(pipeline) : null;
        }

        public async Task<IEnumerable<PipelineMonitoringDto>> GetPipelineMonitoringDataAsync(int pipelineId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            await Task.Delay(1);
            
            var query = _monitoringData.Where(m => m.PipelineId == pipelineId);
            
            if (fromDate.HasValue)
                query = query.Where(m => m.RecordTime >= fromDate.Value);
            
            if (toDate.HasValue)
                query = query.Where(m => m.RecordTime <= toDate.Value);
            
            var monitoringList = query.OrderByDescending(m => m.RecordTime).ToList();
            var result = new List<PipelineMonitoringDto>();
            
            foreach (var monitoring in monitoringList)
            {
                var pipeline = _pipelines.FirstOrDefault(p => p.Id == monitoring.PipelineId);
                if (pipeline != null)
                {
                    result.Add(MapToMonitoringDto(monitoring, pipeline));
                }
            }
            
            return result;
        }

        public async Task<IEnumerable<PipelineAlertDto>> GetPipelineAlertsAsync(bool activeOnly = true)
        {
            await Task.Delay(1);
            
            var query = _alerts.AsQueryable();
            if (activeOnly)
                query = query.Where(a => a.Status == "Active");
            
            var alertList = query.OrderByDescending(a => a.AlertTime).ToList();
            var result = new List<PipelineAlertDto>();
            
            foreach (var alert in alertList)
            {
                var pipeline = _pipelines.FirstOrDefault(p => p.Id == alert.PipelineId);
                if (pipeline != null)
                {
                    result.Add(MapToAlertDto(alert, pipeline));
                }
            }
            
            return result;
        }

        public async Task<IEnumerable<PipelineAlertDto>> GetPipelineAlertsByIdAsync(int pipelineId, bool activeOnly = true)
        {
            await Task.Delay(1);
            
            var query = _alerts.Where(a => a.PipelineId == pipelineId);
            if (activeOnly)
                query = query.Where(a => a.Status == "Active");
            
            var alertList = query.OrderByDescending(a => a.AlertTime).ToList();
            var pipeline = _pipelines.FirstOrDefault(p => p.Id == pipelineId);
            
            return alertList.Select(alert => MapToAlertDto(alert, pipeline!));
        }

        public async Task<PipelineAlertDto> CreateAlertAsync(CreatePipelineAlertDto createAlertDto)
        {
            await Task.Delay(1);
            
            var alert = new PipelineAlert
            {
                Id = _nextAlertId++,
                PipelineId = createAlertDto.PipelineId,
                AlertTime = DateTime.Now,
                AlertType = createAlertDto.AlertType,
                Severity = createAlertDto.Severity,
                Title = createAlertDto.Title,
                Description = createAlertDto.Description,
                Status = "Active",
                IsNotificationSent = false
            };
            
            _alerts.Add(alert);
            
            var pipeline = _pipelines.FirstOrDefault(p => p.Id == alert.PipelineId);
            return MapToAlertDto(alert, pipeline!);
        }

        public async Task<bool> AcknowledgeAlertAsync(int alertId, string acknowledgedBy)
        {
            await Task.Delay(1);
            
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null || alert.Status != "Active") return false;
            
            alert.Status = "Acknowledged";
            alert.AcknowledgedTime = DateTime.Now;
            alert.AcknowledgedBy = acknowledgedBy;
            
            return true;
        }

        public async Task<bool> ResolveAlertAsync(int alertId, string resolvedBy, string resolution)
        {
            await Task.Delay(1);
            
            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null) return false;
            
            alert.Status = "Resolved";
            alert.ResolvedTime = DateTime.Now;
            alert.ResolvedBy = resolvedBy;
            alert.Resolution = resolution;
            
            return true;
        }

        public async Task<PipelineMonitoringDto> AddMonitoringDataAsync(int pipelineId, decimal pressure, decimal flowRate, decimal temperature, decimal humidity)
        {
            await Task.Delay(1);
            
            var monitoring = new PipelineMonitoring
            {
                Id = _nextMonitoringId++,
                PipelineId = pipelineId,
                RecordTime = DateTime.Now,
                Pressure = pressure,
                FlowRate = flowRate,
                Temperature = temperature,
                Humidity = humidity,
                DataSource = "Sensor",
                IsValidData = true
            };
            
            // 檢查是否需要觸發警報
            var pipeline = _pipelines.FirstOrDefault(p => p.Id == pipelineId);
            if (pipeline != null)
            {
                if (pressure > pipeline.MaxPressure * 0.9m) // 超過90%最大壓力
                {
                    monitoring.IsAlarmTriggered = true;
                    monitoring.AlarmType = "Pressure";
                    monitoring.AlarmMessage = $"壓力過高：{pressure} kPa (最大：{pipeline.MaxPressure} kPa)";
                    
                    // 自動建立警報
                    await CreateAlertAsync(new CreatePipelineAlertDto
                    {
                        PipelineId = pipelineId,
                        AlertType = "Pressure",
                        Severity = "High",
                        Title = "管線壓力過高警報",
                        Description = monitoring.AlarmMessage
                    });
                }
                
                if (flowRate < 10) // 流量過低
                {
                    monitoring.IsAlarmTriggered = true;
                    monitoring.AlarmType = "Flow";
                    monitoring.AlarmMessage = $"流量過低：{flowRate} m³/h";
                }
            }
            
            _monitoringData.Add(monitoring);
            
            return MapToMonitoringDto(monitoring, pipeline!);
        }

        public async Task<IEnumerable<PipelineDto>> GetPipelinesByStatusAsync(string status)
        {
            await Task.Delay(1);
            var pipelines = _pipelines.Where(p => p.Status == status);
            return pipelines.Select(MapToPipelineDto);
        }

        private void InitializeSampleData()
        {
            // 初始化管線資料
            var samplePipelines = new List<Pipeline>
            {
                new Pipeline
                {
                    Id = _nextPipelineId++,
                    PipelineNumber = "P001",
                    Name = "主幹線一號",
                    Type = "Main",
                    Length = 25.5m,
                    Diameter = 600,
                    MaxPressure = 1000,
                    OperatingPressure = 800,
                    Material = "鋼管",
                    InstallationDate = DateTime.Now.AddYears(-5),
                    LastInspectionDate = DateTime.Now.AddMonths(-3),
                    NextInspectionDate = DateTime.Now.AddMonths(9),
                    Status = "Active",
                    Location = "台北市信義區",
                    Latitude = 25.033m,
                    Longitude = 121.565m
                },
                new Pipeline
                {
                    Id = _nextPipelineId++,
                    PipelineNumber = "P002",
                    Name = "配送線二號",
                    Type = "Distribution",
                    Length = 12.8m,
                    Diameter = 300,
                    MaxPressure = 500,
                    OperatingPressure = 350,
                    Material = "鋼管",
                    InstallationDate = DateTime.Now.AddYears(-3),
                    LastInspectionDate = DateTime.Now.AddMonths(-1),
                    NextInspectionDate = DateTime.Now.AddMonths(11),
                    Status = "Active",
                    Location = "台北市大安區",
                    Latitude = 25.026m,
                    Longitude = 121.543m
                }
            };
            
            _pipelines.AddRange(samplePipelines);
            
            // 初始化監控資料
            InitializeMonitoringData();
            
            // 初始化警報資料
            InitializeAlertData();
        }
        
        private void InitializeMonitoringData()
        {
            var random = new Random();
            var baseTime = DateTime.Now.AddHours(-24);
            
            for (int i = 0; i < 100; i++)
            {
                foreach (var pipeline in _pipelines)
                {
                    var monitoring = new PipelineMonitoring
                    {
                        Id = _nextMonitoringId++,
                        PipelineId = pipeline.Id,
                        RecordTime = baseTime.AddMinutes(i * 15),
                        Pressure = pipeline.OperatingPressure + (decimal)(random.NextDouble() * 100 - 50),
                        FlowRate = 50 + (decimal)(random.NextDouble() * 100),
                        Temperature = 15 + (decimal)(random.NextDouble() * 20),
                        Humidity = 40 + (decimal)(random.NextDouble() * 40),
                        DataSource = "Sensor",
                        IsValidData = true,
                        IsAlarmTriggered = false
                    };
                    
                    _monitoringData.Add(monitoring);
                }
            }
        }
        
        private void InitializeAlertData()
        {
            var alerts = new List<PipelineAlert>
            {
                new PipelineAlert
                {
                    Id = _nextAlertId++,
                    PipelineId = 1,
                    AlertTime = DateTime.Now.AddHours(-2),
                    AlertType = "Pressure",
                    Severity = "High",
                    Title = "管線壓力異常",
                    Description = "主幹線一號壓力超過安全範圍",
                    Status = "Active",
                    IsNotificationSent = true
                },
                new PipelineAlert
                {
                    Id = _nextAlertId++,
                    PipelineId = 2,
                    AlertTime = DateTime.Now.AddDays(-1),
                    AlertType = "Maintenance",
                    Severity = "Medium",
                    Title = "定期維護提醒",
                    Description = "配送線二號需要進行定期檢查",
                    Status = "Acknowledged",
                    AcknowledgedTime = DateTime.Now.AddHours(-12),
                    AcknowledgedBy = "維護工程師",
                    IsNotificationSent = true
                }
            };
            
            _alerts.AddRange(alerts);
        }

        private static PipelineDto MapToPipelineDto(Pipeline pipeline)
        {
            return new PipelineDto
            {
                Id = pipeline.Id,
                PipelineNumber = pipeline.PipelineNumber,
                Name = pipeline.Name,
                Type = pipeline.Type,
                Length = pipeline.Length,
                Diameter = pipeline.Diameter,
                MaxPressure = pipeline.MaxPressure,
                OperatingPressure = pipeline.OperatingPressure,
                Material = pipeline.Material,
                InstallationDate = pipeline.InstallationDate,
                LastInspectionDate = pipeline.LastInspectionDate,
                NextInspectionDate = pipeline.NextInspectionDate,
                Status = pipeline.Status,
                Location = pipeline.Location,
                Latitude = pipeline.Latitude,
                Longitude = pipeline.Longitude
            };
        }

        private static PipelineMonitoringDto MapToMonitoringDto(PipelineMonitoring monitoring, Pipeline pipeline)
        {
            return new PipelineMonitoringDto
            {
                Id = monitoring.Id,
                PipelineId = monitoring.PipelineId,
                PipelineNumber = pipeline.PipelineNumber,
                PipelineName = pipeline.Name,
                RecordTime = monitoring.RecordTime,
                Pressure = monitoring.Pressure,
                FlowRate = monitoring.FlowRate,
                Temperature = monitoring.Temperature,
                Humidity = monitoring.Humidity,
                IsAlarmTriggered = monitoring.IsAlarmTriggered,
                AlarmType = monitoring.AlarmType,
                AlarmMessage = monitoring.AlarmMessage,
                DataSource = monitoring.DataSource,
                IsValidData = monitoring.IsValidData
            };
        }

        private static PipelineAlertDto MapToAlertDto(PipelineAlert alert, Pipeline pipeline)
        {
            return new PipelineAlertDto
            {
                Id = alert.Id,
                PipelineId = alert.PipelineId,
                PipelineNumber = pipeline.PipelineNumber,
                PipelineName = pipeline.Name,
                AlertTime = alert.AlertTime,
                AlertType = alert.AlertType,
                Severity = alert.Severity,
                Title = alert.Title,
                Description = alert.Description,
                Status = alert.Status,
                AcknowledgedTime = alert.AcknowledgedTime,
                AcknowledgedBy = alert.AcknowledgedBy,
                ResolvedTime = alert.ResolvedTime,
                ResolvedBy = alert.ResolvedBy,
                Resolution = alert.Resolution
            };
        }
    }
}