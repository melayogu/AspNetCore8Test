using AspNetCore8Test.Models.DTOs.GasDTOs;

namespace AspNetCore8Test.Services.GasServices
{
    public interface ILoRaMeterService
    {
        Task<LoRaDashboardViewModel> GetDashboardAsync();
        Task<LoRaOverviewDto> GetOverviewAsync();
        Task<IEnumerable<LoRaDeviceDto>> GetDevicesAsync(string? status = null);
        Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id);
        Task<IEnumerable<LoRaReadingDto>> GetRecentReadingsAsync(int take = 20);
        Task<IEnumerable<LoRaReadingDto>> GetDeviceReadingsAsync(int deviceId, int take = 20);
        Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync();
        Task<IEnumerable<LoRaAlertDto>> GetAlertsAsync(bool includeResolved = false);
        Task<IEnumerable<LoRaNetworkStatusDto>> GetNetworkStatusAsync();
        Task<bool> AcknowledgeAlertAsync(int alertId, string acknowledgedBy);
        Task<bool> ResolveAlertAsync(int alertId, string resolvedBy);
    }

    /// <summary>
    /// 提供 LoRa 無線抄表系統的模擬資料與營運邏輯
    /// </summary>
    public class LoRaMeterService : ILoRaMeterService
    {
        private static readonly List<LoRaDeviceDto> _devices = new();
        private static readonly List<LoRaGatewayDto> _gateways = new();
        private static readonly List<LoRaReadingDto> _readings = new();
        private static readonly List<LoRaAlertDto> _alerts = new();
        private static readonly List<LoRaNetworkStatusDto> _networkStatuses = new();
        private static int _nextReadingId = 1;
        private static int _nextAlertId = 1;
        private static bool _initialized;

        public LoRaMeterService()
        {
            if (!_initialized)
            {
                InitializeSampleData();
                _initialized = true;
            }
        }

        public async Task<LoRaDashboardViewModel> GetDashboardAsync()
        {
            var overview = await GetOverviewAsync();
            var topDevices = _devices
                .OrderByDescending(d => d.SignalStrength)
                .ThenByDescending(d => d.LastReadingTime)
                .Take(5)
                .ToList();

            var recentReadings = _readings
                .OrderByDescending(r => r.ReadingTime)
                .Take(10)
                .ToList();

            var activeAlerts = _alerts
                .Where(a => a.Status != "Resolved")
                .OrderByDescending(a => a.DetectedAt)
                .Take(6)
                .ToList();

            var networkStatus = _networkStatuses.ToList();
            var gateways = _gateways.OrderByDescending(g => g.PacketSuccessRate).Take(4).ToList();

            return new LoRaDashboardViewModel
            {
                Overview = overview,
                TopDevices = topDevices,
                RecentReadings = recentReadings,
                ActiveAlerts = activeAlerts,
                NetworkStatus = networkStatus,
                Gateways = gateways
            };
        }

        public async Task<LoRaOverviewDto> GetOverviewAsync()
        {
            await Task.Delay(1);

            var now = DateTime.Now;
            var lastDay = now.AddDays(-1);
            var lastMonth = now.AddDays(-30);

            var overview = new LoRaOverviewDto
            {
                TotalDevices = _devices.Count,
                ActiveDevices = _devices.Count(d => d.Status == "Active"),
                AlertDevices = _alerts.Count(a => a.Status != "Resolved"),
                AverageSignalStrength = _devices.Any() ? Math.Round(_devices.Average(d => d.SignalStrength), 1) : 0,
                PacketSuccessRate = _gateways.Any() ? Math.Round(_gateways.Average(g => g.PacketSuccessRate), 2) : 0,
                DailyReadings = _readings.Count(r => r.ReadingTime >= lastDay),
                MonthlyReadings = _readings.Count(r => r.ReadingTime >= lastMonth),
                CoveragePercentage = _networkStatuses.Any() ? Math.Round(_networkStatuses.Average(n => n.CoverageRate), 2) : 0,
                GatewayCount = _gateways.Count,
                OfflineGatewayCount = _gateways.Count(g => g.Status != "Online"),
                BatteryCriticalCount = _devices.Count(d => d.BatteryLevel < 20),
                FirmwareOutdatedCount = _devices.Count(d => !d.FirmwareUpToDate),
                RecentAlertCount = _alerts.Count(a => a.Status != "Resolved" && a.DetectedAt >= lastDay),
                GeneratedAt = now
            };

            return overview;
        }

        public async Task<IEnumerable<LoRaDeviceDto>> GetDevicesAsync(string? status = null)
        {
            await Task.Delay(1);

            var query = _devices.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(status) && !status.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return query.OrderBy(d => d.DeviceNumber).ToList();
        }

        public async Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id)
        {
            await Task.Delay(1);
            return _devices.FirstOrDefault(d => d.Id == id);
        }

        public async Task<IEnumerable<LoRaReadingDto>> GetRecentReadingsAsync(int take = 20)
        {
            await Task.Delay(1);
            return _readings
                .OrderByDescending(r => r.ReadingTime)
                .Take(take)
                .ToList();
        }

        public async Task<IEnumerable<LoRaReadingDto>> GetDeviceReadingsAsync(int deviceId, int take = 20)
        {
            await Task.Delay(1);
            return _readings
                .Where(r => r.DeviceId == deviceId)
                .OrderByDescending(r => r.ReadingTime)
                .Take(take)
                .ToList();
        }

        public async Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync()
        {
            await Task.Delay(1);
            return _gateways
                .OrderBy(g => g.Region)
                .ThenBy(g => g.GatewayId)
                .ToList();
        }

        public async Task<IEnumerable<LoRaAlertDto>> GetAlertsAsync(bool includeResolved = false)
        {
            await Task.Delay(1);
            var query = _alerts.AsEnumerable();
            if (!includeResolved)
            {
                query = query.Where(a => a.Status != "Resolved");
            }

            return query
                .OrderByDescending(a => a.DetectedAt)
                .ToList();
        }

        public async Task<IEnumerable<LoRaNetworkStatusDto>> GetNetworkStatusAsync()
        {
            await Task.Delay(1);
            return _networkStatuses.ToList();
        }

        public async Task<bool> AcknowledgeAlertAsync(int alertId, string acknowledgedBy)
        {
            await Task.Delay(1);

            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null || alert.Status == "Resolved")
            {
                return false;
            }

            alert.Status = "Acknowledged";
            alert.AcknowledgedAt = DateTime.Now;
            alert.AcknowledgedBy = acknowledgedBy;
            return true;
        }

        public async Task<bool> ResolveAlertAsync(int alertId, string resolvedBy)
        {
            await Task.Delay(1);

            var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null)
            {
                return false;
            }

            alert.Status = "Resolved";
            alert.ResolvedAt = DateTime.Now;
            alert.ResolvedBy = resolvedBy;
            return true;
        }

        private void InitializeSampleData()
        {
            InitializeGateways();
            InitializeDevices();
            InitializeReadings();
            InitializeAlerts();
            InitializeNetworkStatus();
        }

        private void InitializeGateways()
        {
            var now = DateTime.Now;
            _gateways.AddRange(new[]
            {
                new LoRaGatewayDto
                {
                    Id = 1,
                    GatewayId = "GW-TPE-001",
                    Region = "台北市",
                    Location = "信義區市府路45號",
                    Latitude = 25.0330,
                    Longitude = 121.5654,
                    Status = "Online",
                    LastHeartbeat = now.AddMinutes(-2),
                    ConnectedDevices = 48,
                    PacketSuccessRate = 99.2,
                    AverageSignalStrength = -83.5,
                    UptimePercentage = 99.95,
                    HasBackupPower = true,
                    IpAddress = "10.10.1.11",
                    FirmwareVersion = "v1.4.2"
                },
                new LoRaGatewayDto
                {
                    Id = 2,
                    GatewayId = "GW-TPE-002",
                    Region = "新北市",
                    Location = "板橋區文化路一段200號",
                    Latitude = 25.0135,
                    Longitude = 121.4637,
                    Status = "Online",
                    LastHeartbeat = now.AddMinutes(-5),
                    ConnectedDevices = 36,
                    PacketSuccessRate = 98.7,
                    AverageSignalStrength = -86.1,
                    UptimePercentage = 99.73,
                    HasBackupPower = true,
                    IpAddress = "10.10.1.27",
                    FirmwareVersion = "v1.4.2"
                },
                new LoRaGatewayDto
                {
                    Id = 3,
                    GatewayId = "GW-TPE-003",
                    Region = "桃園市",
                    Location = "桃園區復興路45號",
                    Latitude = 24.9932,
                    Longitude = 121.3008,
                    Status = "Maintenance",
                    LastHeartbeat = now.AddMinutes(-35),
                    ConnectedDevices = 21,
                    PacketSuccessRate = 96.4,
                    AverageSignalStrength = -88.9,
                    UptimePercentage = 98.85,
                    HasBackupPower = false,
                    IpAddress = "10.10.2.5",
                    FirmwareVersion = "v1.3.9"
                }
            });
        }

        private void InitializeDevices()
        {
            var now = DateTime.Now;
            _devices.AddRange(new[]
            {
                new LoRaDeviceDto
                {
                    Id = 1,
                    DeviceNumber = "LORA-0001",
                    MeterNumber = "MTR-001",
                    CustomerName = "台北市信義區松高路住戶",
                    Address = "台北市信義區松高路 100 號",
                    Status = "Active",
                    SignalStrength = -82.4,
                    Snr = 7.8,
                    BatteryLevel = 76,
                    FirmwareVersion = "1.2.3",
                    FirmwareUpToDate = true,
                    LastReadingTime = now.AddMinutes(-25),
                    LastReadingValue = 1284.32m,
                    MonthlyUsage = 32.6m,
                    GatewayId = "GW-TPE-001",
                    AlertStatus = "Normal",
                    HasRecentAlert = false,
                    InstallationType = "智慧瓦斯錶",
                    ActivatedAt = now.AddMonths(-8)
                },
                new LoRaDeviceDto
                {
                    Id = 2,
                    DeviceNumber = "LORA-0002",
                    MeterNumber = "MTR-002",
                    CustomerName = "台北市大安區和平東路住戶",
                    Address = "台北市大安區和平東路二段 56 號",
                    Status = "Active",
                    SignalStrength = -87.1,
                    Snr = 6.5,
                    BatteryLevel = 18,
                    FirmwareVersion = "1.1.9",
                    FirmwareUpToDate = false,
                    LastReadingTime = now.AddMinutes(-40),
                    LastReadingValue = 954.74m,
                    MonthlyUsage = 28.4m,
                    GatewayId = "GW-TPE-001",
                    AlertStatus = "Battery",
                    HasRecentAlert = true,
                    InstallationType = "室內壁掛",
                    ActivatedAt = now.AddMonths(-12)
                },
                new LoRaDeviceDto
                {
                    Id = 3,
                    DeviceNumber = "LORA-0003",
                    MeterNumber = "MTR-003",
                    CustomerName = "新北市板橋區中山路用戶",
                    Address = "新北市板橋區中山路一段 88 號",
                    Status = "Active",
                    SignalStrength = -79.6,
                    Snr = 8.2,
                    BatteryLevel = 64,
                    FirmwareVersion = "1.2.3",
                    FirmwareUpToDate = true,
                    LastReadingTime = now.AddMinutes(-18),
                    LastReadingValue = 1120.18m,
                    MonthlyUsage = 24.1m,
                    GatewayId = "GW-TPE-002",
                    AlertStatus = "Normal",
                    HasRecentAlert = false,
                    InstallationType = "戶外箱體",
                    ActivatedAt = now.AddMonths(-5)
                },
                new LoRaDeviceDto
                {
                    Id = 4,
                    DeviceNumber = "LORA-0004",
                    MeterNumber = "MTR-004",
                    CustomerName = "桃園市中壢區企業用戶",
                    Address = "桃園市中壢區中正路 300 號",
                    Status = "Maintenance",
                    SignalStrength = -90.3,
                    Snr = 5.9,
                    BatteryLevel = 52,
                    FirmwareVersion = "1.2.1",
                    FirmwareUpToDate = true,
                    LastReadingTime = now.AddHours(-3),
                    LastReadingValue = 3420.52m,
                    MonthlyUsage = 88.3m,
                    GatewayId = "GW-TPE-003",
                    AlertStatus = "Intermittent",
                    HasRecentAlert = true,
                    InstallationType = "工業型智慧錶",
                    ActivatedAt = now.AddMonths(-18)
                },
                new LoRaDeviceDto
                {
                    Id = 5,
                    DeviceNumber = "LORA-0005",
                    MeterNumber = "MTR-005",
                    CustomerName = "新北市永和區民樂街住戶",
                    Address = "新北市永和區民樂街 12 號",
                    Status = "Active",
                    SignalStrength = -85.7,
                    Snr = 7.1,
                    BatteryLevel = 34,
                    FirmwareVersion = "1.2.3",
                    FirmwareUpToDate = true,
                    LastReadingTime = now.AddMinutes(-55),
                    LastReadingValue = 765.33m,
                    MonthlyUsage = 19.6m,
                    GatewayId = "GW-TPE-002",
                    AlertStatus = "Normal",
                    HasRecentAlert = false,
                    InstallationType = "智慧瓦斯錶",
                    ActivatedAt = now.AddMonths(-10)
                }
            });
        }

        private void InitializeReadings()
        {
            var random = new Random(1024);
            var now = DateTime.Now;

            foreach (var device in _devices)
            {
                decimal readingBase = device.LastReadingValue - 50m;
                for (int i = 0; i < 48; i++)
                {
                    var timestamp = now.AddMinutes(-i * 30);
                    var usage = Math.Round((decimal)(random.NextDouble() * 2.5 + 0.4), 2);
                    readingBase += usage;
                    var signalDrift = random.NextDouble() * 4 - 2;
                    var snrDrift = random.NextDouble() * 1.5 - 0.75;
                    var batteryDrain = Math.Max(device.BatteryLevel - i / 24 - random.Next(0, 2), 5);

                    var reading = new LoRaReadingDto
                    {
                        Id = _nextReadingId++,
                        DeviceId = device.Id,
                        DeviceNumber = device.DeviceNumber,
                        MeterNumber = device.MeterNumber,
                        CustomerName = device.CustomerName,
                        ReadingTime = timestamp,
                        ReadingValue = Math.Round(readingBase, 2),
                        Usage = usage,
                        SignalStrength = Math.Round(device.SignalStrength + signalDrift, 1),
                        Snr = Math.Round(device.Snr + snrDrift, 1),
                        BatteryLevel = (int)batteryDrain,
                        IsAlert = false,
                        AlertType = string.Empty,
                        TransmissionStatus = signalDrift < -3 || device.Status != "Active" ? "Retry" : "Delivered",
                        GatewayId = device.GatewayId,
                        PacketLoss = Math.Round(random.NextDouble() * 1.5, 2)
                    };

                    // 標記異常情況
                    if (i == 0 && device.BatteryLevel < 20)
                    {
                        reading.IsAlert = true;
                        reading.AlertType = "LowBattery";
                    }

                    if (i == 0 && device.Status == "Maintenance")
                    {
                        reading.IsAlert = true;
                        reading.AlertType = "SignalDrop";
                        reading.TransmissionStatus = "Pending";
                    }

                    _readings.Add(reading);
                }
            }
        }

        private void InitializeAlerts()
        {
            var now = DateTime.Now;
            _alerts.AddRange(new[]
            {
                new LoRaAlertDto
                {
                    Id = _nextAlertId++,
                    DeviceId = 2,
                    DeviceNumber = "LORA-0002",
                    MeterNumber = "MTR-002",
                    CustomerName = "台北市大安區和平東路住戶",
                    AlertType = "Battery",
                    Severity = "High",
                    Status = "Active",
                    DetectedAt = now.AddHours(-2),
                    Description = "裝置電量低於 20%，請安排更換。",
                    RecommendedAction = "指派維運人員於 24 小時內更換電池"
                },
                new LoRaAlertDto
                {
                    Id = _nextAlertId++,
                    DeviceId = 4,
                    DeviceNumber = "LORA-0004",
                    MeterNumber = "MTR-004",
                    CustomerName = "桃園市中壢區企業用戶",
                    AlertType = "Signal",
                    Severity = "Medium",
                    Status = "Acknowledged",
                    DetectedAt = now.AddHours(-6),
                    Description = "最近 3 小時訊號品質不穩定。",
                    RecommendedAction = "檢查閘道器與天線狀態",
                    AcknowledgedAt = now.AddHours(-5),
                    AcknowledgedBy = "系統值班工程師"
                },
                new LoRaAlertDto
                {
                    Id = _nextAlertId++,
                    DeviceId = 1,
                    DeviceNumber = "LORA-0001",
                    MeterNumber = "MTR-001",
                    CustomerName = "台北市信義區松高路住戶",
                    AlertType = "Firmware",
                    Severity = "Low",
                    Status = "Resolved",
                    DetectedAt = now.AddDays(-1),
                    Description = "韌體版本落後一個版本。",
                    RecommendedAction = "透過遠端推送更新",
                    AcknowledgedAt = now.AddHours(-20),
                    AcknowledgedBy = "值班工程師",
                    ResolvedAt = now.AddHours(-18),
                    ResolvedBy = "自動化更新程序"
                }
            });
        }

        private void InitializeNetworkStatus()
        {
            _networkStatuses.AddRange(new[]
            {
                new LoRaNetworkStatusDto
                {
                    Region = "台北市",
                    DeviceCount = 120,
                    AverageSignalStrength = -84.2,
                    PacketSuccessRate = 99.1,
                    CoverageRate = 98.7,
                    ActiveGateways = 2,
                    AlertCount = 3
                },
                new LoRaNetworkStatusDto
                {
                    Region = "新北市",
                    DeviceCount = 95,
                    AverageSignalStrength = -86.9,
                    PacketSuccessRate = 98.4,
                    CoverageRate = 97.3,
                    ActiveGateways = 2,
                    AlertCount = 2
                },
                new LoRaNetworkStatusDto
                {
                    Region = "桃園市",
                    DeviceCount = 58,
                    AverageSignalStrength = -89.5,
                    PacketSuccessRate = 96.8,
                    CoverageRate = 95.2,
                    ActiveGateways = 1,
                    AlertCount = 1
                }
            });
        }
    }
}
