using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface ILoRaSystemService
    {
        Task<LoRaDashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync();
        Task<IEnumerable<LoRaDeviceDto>> GetDevicesAsync(string? status = null, string? search = null, int? gatewayId = null);
        Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id);
        Task<LoRaDeviceDto> AddDeviceAsync(CreateLoRaDeviceDto dto);
        Task<bool> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto dto);
        Task<bool> RemoveDeviceAsync(int id);
        Task<IEnumerable<LoRaMeterReadingDto>> GetLatestReadingsAsync(int? deviceId = null, int limit = 20);
        Task<IEnumerable<LoRaMeterReadingDto>> GetDeviceReadingsAsync(int deviceId, DateTime? from = null, DateTime? to = null);
        Task<LoRaMeterReadingDto> AddMeterReadingAsync(CreateLoRaMeterReadingDto dto);
        Task<IEnumerable<LoRaAlertDto>> GetAlertsAsync(string? status = null, int? deviceId = null);
        Task<LoRaAlertDto> CreateAlertAsync(CreateLoRaAlertDto dto);
        Task<bool> UpdateAlertStatusAsync(int alertId, UpdateLoRaAlertStatusDto dto);
    }

    public class LoRaSystemService : ILoRaSystemService
    {
        private static readonly List<LoRaGateway> _gateways = new();
        private static readonly List<LoRaDevice> _devices = new();
        private static readonly List<LoRaMeterReading> _readings = new();
        private static readonly List<LoRaAlert> _alerts = new();

        private static int _nextGatewayId = 1;
        private static int _nextDeviceId = 1;
        private static int _nextReadingId = 1;
        private static int _nextAlertId = 1;

        private readonly object _lock = new();

        public LoRaSystemService()
        {
            if (!_gateways.Any())
            {
                lock (_lock)
                {
                    if (!_gateways.Any())
                    {
                        InitializeSampleData();
                    }
                }
            }
        }

        public Task<LoRaDashboardStatsDto> GetDashboardStatsAsync()
        {
            lock (_lock)
            {
                var devices = _devices.ToList();
                var gateways = _gateways.ToList();
                var alerts = _alerts.ToList();
                var readings = _readings.ToList();
                var now = DateTime.UtcNow;

                var stats = new LoRaDashboardStatsDto
                {
                    TotalDevices = devices.Count,
                    ActiveDevices = devices.Count(d => string.Equals(d.Status, "Active", StringComparison.OrdinalIgnoreCase)),
                    MaintenanceDevices = devices.Count(d => string.Equals(d.Status, "Maintenance", StringComparison.OrdinalIgnoreCase)),
                    OfflineDevices = devices.Count(d => string.Equals(d.Status, "Offline", StringComparison.OrdinalIgnoreCase)),
                    TotalGateways = gateways.Count,
                    GatewayOnline = gateways.Count(g => string.Equals(g.Status, "Online", StringComparison.OrdinalIgnoreCase)),
                    GatewayOffline = gateways.Count(g => !string.Equals(g.Status, "Online", StringComparison.OrdinalIgnoreCase)),
                    AverageBatteryLevel = devices.Any() ? Math.Round((double)devices.Average(d => d.BatteryLevel), 1) : 0,
                    AverageSignalStrength = devices.Any() ? Math.Round(devices.Average(d => d.SignalStrength), 1) : 0,
                    ActiveAlerts = alerts.Count(a => string.Equals(a.Status, "Active", StringComparison.OrdinalIgnoreCase)),
                    DailyReadings = readings.Count(r => r.ReadingTime.Date == now.Date),
                    MonthlyReadings = readings.Count(r => r.ReadingTime >= new DateTime(now.Year, now.Month, 1))
                };

                return Task.FromResult(stats);
            }
        }

        public Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync()
        {
            lock (_lock)
            {
                var result = _gateways
                    .Select(MapToGatewayDto)
                    .OrderByDescending(g => g.LastHeartbeat)
                    .ToList()
                    .AsEnumerable();

                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<LoRaDeviceDto>> GetDevicesAsync(string? status = null, string? search = null, int? gatewayId = null)
        {
            lock (_lock)
            {
                var query = _devices.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(d => string.Equals(d.Status, status, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(d =>
                        d.DeviceName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        d.DeviceCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        d.Location.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        d.MeterType.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                if (gatewayId.HasValue)
                {
                    query = query.Where(d => d.GatewayId == gatewayId.Value);
                }

                var result = query
                    .Select(MapToDeviceDto)
                    .OrderByDescending(d => d.LastCommunication)
                    .ToList()
                    .AsEnumerable();

                return Task.FromResult(result);
            }
        }

        public Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id)
        {
            lock (_lock)
            {
                var device = _devices.FirstOrDefault(d => d.Id == id);
                return Task.FromResult(device != null ? MapToDeviceDto(device) : null);
            }
        }

        public Task<LoRaDeviceDto> AddDeviceAsync(CreateLoRaDeviceDto dto)
        {
            lock (_lock)
            {
                var device = new LoRaDevice
                {
                    Id = _nextDeviceId++,
                    DeviceCode = dto.DeviceCode,
                    DeviceName = dto.DeviceName,
                    MeterType = dto.MeterType,
                    Location = dto.Location,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    GatewayId = dto.GatewayId,
                    Status = string.IsNullOrWhiteSpace(dto.Status) ? "Active" : dto.Status!,
                    InstallDate = dto.InstallDate ?? DateTime.UtcNow.Date,
                    FirmwareVersion = string.IsNullOrWhiteSpace(dto.FirmwareVersion) ? "v1.0.0" : dto.FirmwareVersion!,
                    BatteryLevel = dto.InitialBatteryLevel.HasValue ? Math.Clamp(dto.InitialBatteryLevel.Value, 0m, 100m) : 100m,
                    SignalStrength = dto.InitialSignalStrength ?? -80,
                    Snr = dto.InitialSnr ?? 8m,
                    TransmissionIntervalMinutes = dto.TransmissionIntervalMinutes > 0 ? dto.TransmissionIntervalMinutes : 15,
                    SupportsValveControl = dto.SupportsValveControl,
                    SupportsTwoWayCommunication = dto.SupportsTwoWayCommunication,
                    Notes = dto.Notes,
                    LastCommunication = DateTime.UtcNow
                };

                _devices.Add(device);

                return Task.FromResult(MapToDeviceDto(device));
            }
        }

        public Task<bool> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto dto)
        {
            lock (_lock)
            {
                var device = _devices.FirstOrDefault(d => d.Id == id);
                if (device == null)
                {
                    return Task.FromResult(false);
                }

                device.DeviceName = dto.DeviceName;
                device.MeterType = dto.MeterType;
                device.Location = dto.Location;
                device.Latitude = dto.Latitude;
                device.Longitude = dto.Longitude;
                device.GatewayId = dto.GatewayId;
                device.Status = dto.Status;
                device.FirmwareVersion = dto.FirmwareVersion;
                device.BatteryLevel = Math.Clamp(dto.BatteryLevel, 0m, 100m);
                device.SignalStrength = dto.SignalStrength;
                device.Snr = dto.Snr;
                device.TransmissionIntervalMinutes = dto.TransmissionIntervalMinutes;
                device.SupportsValveControl = dto.SupportsValveControl;
                device.SupportsTwoWayCommunication = dto.SupportsTwoWayCommunication;
                device.Notes = dto.Notes;
                device.LastCommunication = DateTime.UtcNow;

                return Task.FromResult(true);
            }
        }

        public Task<bool> RemoveDeviceAsync(int id)
        {
            lock (_lock)
            {
                var device = _devices.FirstOrDefault(d => d.Id == id);
                if (device == null)
                {
                    return Task.FromResult(false);
                }

                _devices.Remove(device);
                _readings.RemoveAll(r => r.DeviceId == id);
                _alerts.RemoveAll(a => a.DeviceId == id);

                return Task.FromResult(true);
            }
        }

        public Task<IEnumerable<LoRaMeterReadingDto>> GetLatestReadingsAsync(int? deviceId = null, int limit = 20)
        {
            lock (_lock)
            {
                var query = _readings.AsEnumerable();

                if (deviceId.HasValue)
                {
                    query = query.Where(r => r.DeviceId == deviceId.Value);
                }

                var result = query
                    .OrderByDescending(r => r.ReadingTime)
                    .Take(limit)
                    .Select(MapToMeterReadingDto)
                    .ToList()
                    .AsEnumerable();

                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<LoRaMeterReadingDto>> GetDeviceReadingsAsync(int deviceId, DateTime? from = null, DateTime? to = null)
        {
            lock (_lock)
            {
                var query = _readings.Where(r => r.DeviceId == deviceId);

                if (from.HasValue)
                {
                    query = query.Where(r => r.ReadingTime >= from.Value);
                }

                if (to.HasValue)
                {
                    query = query.Where(r => r.ReadingTime <= to.Value);
                }

                var result = query
                    .OrderByDescending(r => r.ReadingTime)
                    .Select(MapToMeterReadingDto)
                    .ToList()
                    .AsEnumerable();

                return Task.FromResult(result);
            }
        }

        public Task<LoRaMeterReadingDto> AddMeterReadingAsync(CreateLoRaMeterReadingDto dto)
        {
            lock (_lock)
            {
                var device = _devices.FirstOrDefault(d => d.Id == dto.DeviceId)
                    ?? throw new ArgumentException($"找不到編號為 {dto.DeviceId} 的 LoRa 設備");

                var reading = new LoRaMeterReading
                {
                    Id = _nextReadingId++,
                    DeviceId = device.Id,
                    ReadingTime = dto.ReadingTime ?? DateTime.UtcNow,
                    Consumption = dto.Consumption,
                    Temperature = dto.Temperature,
                    BatteryLevel = dto.BatteryLevel,
                    BatteryVoltage = dto.BatteryVoltage,
                    Rssi = dto.Rssi,
                    Snr = dto.Snr,
                    PacketLossRate = dto.PacketLossRate,
                    ValveStatus = dto.ValveStatus,
                    TamperStatus = dto.TamperStatus,
                    IsConfirmed = dto.IsConfirmed,
                    DataRate = string.IsNullOrWhiteSpace(dto.DataRate) ? "DR2" : dto.DataRate
                };

                _readings.Add(reading);

                device.LastReadingValue = reading.Consumption;
                device.LastReadingAt = reading.ReadingTime;
                device.LastCommunication = reading.ReadingTime;

                if (reading.BatteryLevel.HasValue)
                {
                    device.BatteryLevel = Math.Clamp(reading.BatteryLevel.Value, 0m, 100m);
                }

                if (reading.Rssi.HasValue)
                {
                    device.SignalStrength = reading.Rssi.Value;
                }

                if (reading.Snr.HasValue)
                {
                    device.Snr = reading.Snr.Value;
                }

                EvaluateReadingForAlerts(device, reading);

                return Task.FromResult(MapToMeterReadingDto(reading));
            }
        }

        public Task<IEnumerable<LoRaAlertDto>> GetAlertsAsync(string? status = null, int? deviceId = null)
        {
            lock (_lock)
            {
                var query = _alerts.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(a => string.Equals(a.Status, status, StringComparison.OrdinalIgnoreCase));
                }

                if (deviceId.HasValue)
                {
                    query = query.Where(a => a.DeviceId == deviceId.Value);
                }

                var result = query
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(MapToAlertDto)
                    .ToList()
                    .AsEnumerable();

                return Task.FromResult(result);
            }
        }

        public Task<LoRaAlertDto> CreateAlertAsync(CreateLoRaAlertDto dto)
        {
            lock (_lock)
            {
                var device = _devices.FirstOrDefault(d => d.Id == dto.DeviceId)
                    ?? throw new ArgumentException($"找不到編號為 {dto.DeviceId} 的 LoRa 設備");

                var alert = new LoRaAlert
                {
                    Id = _nextAlertId++,
                    DeviceId = device.Id,
                    AlertType = dto.AlertType,
                    Severity = dto.Severity,
                    Title = string.IsNullOrWhiteSpace(dto.Title) ? dto.AlertType : dto.Title,
                    Message = dto.Message,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                };

                _alerts.Add(alert);

                return Task.FromResult(MapToAlertDto(alert));
            }
        }

        public Task<bool> UpdateAlertStatusAsync(int alertId, UpdateLoRaAlertStatusDto dto)
        {
            lock (_lock)
            {
                var alert = _alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert == null)
                {
                    return Task.FromResult(false);
                }

                alert.Status = dto.Status;

                if (string.Equals(dto.Status, "Acknowledged", StringComparison.OrdinalIgnoreCase))
                {
                    alert.AcknowledgedAt = DateTime.UtcNow;
                    alert.AcknowledgedBy = string.IsNullOrWhiteSpace(dto.ResolvedBy) ? "系統" : dto.ResolvedBy;
                }

                if (string.Equals(dto.Status, "Resolved", StringComparison.OrdinalIgnoreCase))
                {
                    alert.ResolvedAt = DateTime.UtcNow;
                    alert.ResolvedBy = string.IsNullOrWhiteSpace(dto.ResolvedBy) ? "系統" : dto.ResolvedBy;
                    alert.ResolutionNotes = dto.ResolutionNotes;
                }

                return Task.FromResult(true);
            }
        }

        private void InitializeSampleData()
        {
            var gatewayNorth = new LoRaGateway
            {
                Id = _nextGatewayId++,
                GatewayCode = "GW-NORTH-001",
                Name = "內湖智慧水表閘道器",
                Location = "台北市內湖區內湖路一段 300 號",
                Latitude = 25.0806,
                Longitude = 121.5755,
                Status = "Online",
                LastHeartbeat = DateTime.UtcNow.AddMinutes(-2),
                InstalledDate = DateTime.UtcNow.AddMonths(-10),
                FirmwareVersion = "v2.4.3",
                ChannelPlan = "AS923-1",
                UplinkFrequency = "923.2 MHz",
                DownlinkFrequency = "923.2 MHz",
                CoverageRadiusKm = 2.3m,
                BackhaulType = "4G LTE",
                PacketForwardSuccessRate = 98.7
            };

            var gatewayCentral = new LoRaGateway
            {
                Id = _nextGatewayId++,
                GatewayCode = "GW-CENTRAL-002",
                Name = "中山區集中器",
                Location = "台北市中山區南京東路三段 120 號",
                Latitude = 25.0514,
                Longitude = 121.5438,
                Status = "Online",
                LastHeartbeat = DateTime.UtcNow.AddMinutes(-5),
                InstalledDate = DateTime.UtcNow.AddMonths(-14),
                FirmwareVersion = "v2.3.9",
                ChannelPlan = "AS923-1",
                UplinkFrequency = "923.4 MHz",
                DownlinkFrequency = "923.4 MHz",
                CoverageRadiusKm = 2.8m,
                BackhaulType = "光纖",
                PacketForwardSuccessRate = 97.9
            };

            var gatewaySouth = new LoRaGateway
            {
                Id = _nextGatewayId++,
                GatewayCode = "GW-SOUTH-003",
                Name = "新店南區閘道器",
                Location = "新北市新店區北宜路二段 200 號",
                Latitude = 24.9598,
                Longitude = 121.5386,
                Status = "Maintenance",
                LastHeartbeat = DateTime.UtcNow.AddMinutes(-32),
                InstalledDate = DateTime.UtcNow.AddMonths(-20),
                FirmwareVersion = "v2.1.4",
                ChannelPlan = "AS923-1",
                UplinkFrequency = "923.0 MHz",
                DownlinkFrequency = "923.0 MHz",
                CoverageRadiusKm = 3.1m,
                BackhaulType = "微波",
                PacketForwardSuccessRate = 95.2
            };

            _gateways.AddRange(new[] { gatewayNorth, gatewayCentral, gatewaySouth });

            var deviceA = new LoRaDevice
            {
                Id = _nextDeviceId++,
                DeviceCode = "LORA-WM-0001",
                DeviceName = "內湖 1 號智慧水表",
                MeterType = "Water",
                Location = "內湖區康寧路三段 200 巷 12 號",
                Latitude = 25.0839,
                Longitude = 121.6112,
                GatewayId = gatewayNorth.Id,
                Status = "Active",
                InstallDate = DateTime.UtcNow.AddMonths(-11),
                FirmwareVersion = "v1.2.5",
                BatteryLevel = 86.5m,
                SignalStrength = -84,
                Snr = 9.4m,
                TransmissionIntervalMinutes = 30,
                SupportsValveControl = true,
                SupportsTwoWayCommunication = true,
                Notes = "地下室安裝，具備閥控功能",
                LastCommunication = DateTime.UtcNow.AddMinutes(-8),
                LastReadingValue = 128.6m,
                LastReadingAt = DateTime.UtcNow.AddHours(-3)
            };

            var deviceB = new LoRaDevice
            {
                Id = _nextDeviceId++,
                DeviceCode = "LORA-GM-0205",
                DeviceName = "民生社區瓦斯表",
                MeterType = "Gas",
                Location = "松山區民生東路五段 250 巷 18 弄 5 號",
                Latitude = 25.0607,
                Longitude = 121.5579,
                GatewayId = gatewayCentral.Id,
                Status = "Active",
                InstallDate = DateTime.UtcNow.AddMonths(-7),
                FirmwareVersion = "v1.3.1",
                BatteryLevel = 72.3m,
                SignalStrength = -92,
                Snr = 7.6m,
                TransmissionIntervalMinutes = 20,
                SupportsValveControl = false,
                SupportsTwoWayCommunication = true,
                Notes = "一般戶，每日 72 筆回傳",
                LastCommunication = DateTime.UtcNow.AddMinutes(-12),
                LastReadingValue = 412.2m,
                LastReadingAt = DateTime.UtcNow.AddHours(-2)
            };

            var deviceC = new LoRaDevice
            {
                Id = _nextDeviceId++,
                DeviceCode = "LORA-EM-1052",
                DeviceName = "新店電力抄表",
                MeterType = "Electric",
                Location = "新店區安和路一段 230 號",
                Latitude = 24.9673,
                Longitude = 121.5409,
                GatewayId = gatewaySouth.Id,
                Status = "Maintenance",
                InstallDate = DateTime.UtcNow.AddMonths(-18),
                FirmwareVersion = "v1.1.8",
                BatteryLevel = 54.9m,
                SignalStrength = -110,
                Snr = 4.2m,
                TransmissionIntervalMinutes = 60,
                SupportsValveControl = false,
                SupportsTwoWayCommunication = false,
                Notes = "施工現場，訊號偏弱",
                LastCommunication = DateTime.UtcNow.AddMinutes(-35),
                LastReadingValue = 9824.4m,
                LastReadingAt = DateTime.UtcNow.AddHours(-5)
            };

            var deviceD = new LoRaDevice
            {
                Id = _nextDeviceId++,
                DeviceCode = "LORA-WM-0310",
                DeviceName = "士林市場水表",
                MeterType = "Water",
                Location = "士林區大南路 84 號",
                Latitude = 25.0914,
                Longitude = 121.5240,
                GatewayId = gatewayCentral.Id,
                Status = "Offline",
                InstallDate = DateTime.UtcNow.AddMonths(-24),
                FirmwareVersion = "v1.0.9",
                BatteryLevel = 18.1m,
                SignalStrength = -118,
                Snr = 0.3m,
                TransmissionIntervalMinutes = 45,
                SupportsValveControl = true,
                SupportsTwoWayCommunication = true,
                Notes = "需安排現場巡檢",
                LastCommunication = DateTime.UtcNow.AddHours(-12),
                LastReadingValue = 256.7m,
                LastReadingAt = DateTime.UtcNow.AddHours(-12)
            };

            _devices.AddRange(new[] { deviceA, deviceB, deviceC, deviceD });

            AddSampleReadings(deviceA, new List<LoRaMeterReading>
            {
                new LoRaMeterReading
                {
                    DeviceId = deviceA.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-18),
                    Consumption = 120.4m,
                    Temperature = 24.6m,
                    BatteryLevel = 88.2m,
                    BatteryVoltage = 3.64m,
                    Rssi = -86,
                    Snr = 9.8m,
                    PacketLossRate = 0.2m,
                    ValveStatus = "Open",
                    TamperStatus = "Normal",
                    IsConfirmed = true,
                    DataRate = "DR2"
                },
                new LoRaMeterReading
                {
                    DeviceId = deviceA.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-6),
                    Consumption = 128.6m,
                    Temperature = 25.1m,
                    BatteryLevel = 86.5m,
                    BatteryVoltage = 3.61m,
                    Rssi = -84,
                    Snr = 9.4m,
                    PacketLossRate = 0.3m,
                    ValveStatus = "Open",
                    TamperStatus = "Normal",
                    IsConfirmed = true,
                    DataRate = "DR3"
                }
            });

            AddSampleReadings(deviceB, new List<LoRaMeterReading>
            {
                new LoRaMeterReading
                {
                    DeviceId = deviceB.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-10),
                    Consumption = 405.9m,
                    Temperature = 26.2m,
                    BatteryLevel = 74.1m,
                    BatteryVoltage = 3.59m,
                    Rssi = -95,
                    Snr = 7.1m,
                    PacketLossRate = 0.6m,
                    ValveStatus = "N/A",
                    TamperStatus = "Normal",
                    IsConfirmed = true,
                    DataRate = "DR2"
                },
                new LoRaMeterReading
                {
                    DeviceId = deviceB.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-2),
                    Consumption = 412.2m,
                    Temperature = 27.0m,
                    BatteryLevel = 72.3m,
                    BatteryVoltage = 3.56m,
                    Rssi = -92,
                    Snr = 7.6m,
                    PacketLossRate = 0.4m,
                    ValveStatus = "N/A",
                    TamperStatus = "Normal",
                    IsConfirmed = true,
                    DataRate = "DR3"
                }
            });

            AddSampleReadings(deviceC, new List<LoRaMeterReading>
            {
                new LoRaMeterReading
                {
                    DeviceId = deviceC.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-13),
                    Consumption = 9788.9m,
                    Temperature = 30.4m,
                    BatteryLevel = 56.2m,
                    BatteryVoltage = 3.42m,
                    Rssi = -112,
                    Snr = 4.6m,
                    PacketLossRate = 2.5m,
                    ValveStatus = "N/A",
                    TamperStatus = "Normal",
                    IsConfirmed = true,
                    DataRate = "DR1"
                },
                new LoRaMeterReading
                {
                    DeviceId = deviceC.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-5),
                    Consumption = 9824.4m,
                    Temperature = 31.1m,
                    BatteryLevel = 54.9m,
                    BatteryVoltage = 3.39m,
                    Rssi = -110,
                    Snr = 4.2m,
                    PacketLossRate = 3.1m,
                    ValveStatus = "N/A",
                    TamperStatus = "DoorOpen",
                    IsConfirmed = false,
                    DataRate = "DR1"
                }
            });

            AddSampleReadings(deviceD, new List<LoRaMeterReading>
            {
                new LoRaMeterReading
                {
                    DeviceId = deviceD.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-26),
                    Consumption = 248.1m,
                    Temperature = 23.5m,
                    BatteryLevel = 23.4m,
                    BatteryVoltage = 3.22m,
                    Rssi = -118,
                    Snr = 1.4m,
                    PacketLossRate = 8.2m,
                    ValveStatus = "Closed",
                    TamperStatus = "Normal",
                    IsConfirmed = false,
                    DataRate = "DR1"
                },
                new LoRaMeterReading
                {
                    DeviceId = deviceD.Id,
                    ReadingTime = DateTime.UtcNow.AddHours(-12),
                    Consumption = 256.7m,
                    Temperature = 24.0m,
                    BatteryLevel = 18.1m,
                    BatteryVoltage = 3.14m,
                    Rssi = -122,
                    Snr = 0.3m,
                    PacketLossRate = 12.4m,
                    ValveStatus = "Closed",
                    TamperStatus = "Tilt",
                    IsConfirmed = false,
                    DataRate = "DR0"
                }
            });

            var alertLowBattery = new LoRaAlert
            {
                Id = _nextAlertId++,
                DeviceId = deviceD.Id,
                AlertType = "Battery",
                Severity = "High",
                Title = "電池電量低於 20%",
                Message = "目前電池僅剩 18%，請安排更換。",
                CreatedAt = DateTime.UtcNow.AddHours(-11),
                Status = "Active"
            };

            var alertTamper = new LoRaAlert
            {
                Id = _nextAlertId++,
                DeviceId = deviceC.Id,
                AlertType = "Tamper",
                Severity = "Medium",
                Title = "疑似箱門開啟",
                Message = "設備回報 Tamper 狀態為 DoorOpen，請派員確認。",
                CreatedAt = DateTime.UtcNow.AddHours(-4),
                Status = "Acknowledged",
                AcknowledgedAt = DateTime.UtcNow.AddHours(-3),
                AcknowledgedBy = "巡檢系統"
            };

            var alertSignal = new LoRaAlert
            {
                Id = _nextAlertId++,
                DeviceId = deviceC.Id,
                AlertType = "Signal",
                Severity = "Medium",
                Title = "訊號強度偏弱",
                Message = "最近 24 小時平均 RSSI -111 dBm。",
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                Status = "Active"
            };

            _alerts.AddRange(new[] { alertLowBattery, alertTamper, alertSignal });
        }

        private void AddSampleReadings(LoRaDevice device, IEnumerable<LoRaMeterReading> readings)
        {
            foreach (var reading in readings)
            {
                reading.Id = _nextReadingId++;
                _readings.Add(reading);

                device.LastReadingValue = reading.Consumption;
                device.LastReadingAt = reading.ReadingTime;

                if (reading.BatteryLevel.HasValue)
                {
                    device.BatteryLevel = Math.Clamp(reading.BatteryLevel.Value, 0m, 100m);
                }

                if (reading.Rssi.HasValue)
                {
                    device.SignalStrength = reading.Rssi.Value;
                }

                if (reading.Snr.HasValue)
                {
                    device.Snr = reading.Snr.Value;
                }

                if (device.LastCommunication < reading.ReadingTime)
                {
                    device.LastCommunication = reading.ReadingTime;
                }
            }
        }

        private void EvaluateReadingForAlerts(LoRaDevice device, LoRaMeterReading reading)
        {
            if (reading.BatteryLevel.HasValue && reading.BatteryLevel.Value < 20m)
            {
                _alerts.Add(new LoRaAlert
                {
                    Id = _nextAlertId++,
                    DeviceId = device.Id,
                    AlertType = "Battery",
                    Severity = "High",
                    Title = "電池電量不足",
                    Message = $"電池電量降至 {reading.BatteryLevel:0.0}%，請安排更換。",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                });
            }

            if (reading.Rssi.HasValue && reading.Rssi.Value < -115)
            {
                _alerts.Add(new LoRaAlert
                {
                    Id = _nextAlertId++,
                    DeviceId = device.Id,
                    AlertType = "Signal",
                    Severity = "Medium",
                    Title = "訊號過低",
                    Message = $"目前 RSSI 為 {reading.Rssi} dBm，需檢查天線或遮蔽物。",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                });
            }

            if (reading.PacketLossRate.HasValue && reading.PacketLossRate.Value > 10m)
            {
                _alerts.Add(new LoRaAlert
                {
                    Id = _nextAlertId++,
                    DeviceId = device.Id,
                    AlertType = "Network",
                    Severity = "Medium",
                    Title = "封包遺失率偏高",
                    Message = $"偵測到封包遺失率 {reading.PacketLossRate:0.0}% ，請檢查通訊品質。",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active"
                });
            }
        }

        private LoRaDeviceDto MapToDeviceDto(LoRaDevice device)
        {
            var gateway = device.GatewayId.HasValue
                ? _gateways.FirstOrDefault(g => g.Id == device.GatewayId.Value)
                : null;

            return new LoRaDeviceDto
            {
                Id = device.Id,
                DeviceCode = device.DeviceCode,
                DeviceName = device.DeviceName,
                MeterType = device.MeterType,
                Location = device.Location,
                Latitude = device.Latitude,
                Longitude = device.Longitude,
                BatteryLevel = device.BatteryLevel,
                SignalStrength = device.SignalStrength,
                Snr = device.Snr,
                FirmwareVersion = device.FirmwareVersion,
                Status = device.Status,
                InstallDate = device.InstallDate,
                LastCommunication = device.LastCommunication,
                GatewayId = device.GatewayId,
                GatewayName = gateway?.Name,
                TransmissionIntervalMinutes = device.TransmissionIntervalMinutes,
                SupportsValveControl = device.SupportsValveControl,
                SupportsTwoWayCommunication = device.SupportsTwoWayCommunication,
                LastReadingValue = device.LastReadingValue,
                LastReadingAt = device.LastReadingAt,
                Notes = device.Notes
            };
        }

        private LoRaGatewayDto MapToGatewayDto(LoRaGateway gateway)
        {
            var connectedDevices = _devices.Count(d => d.GatewayId == gateway.Id);

            return new LoRaGatewayDto
            {
                Id = gateway.Id,
                GatewayCode = gateway.GatewayCode,
                Name = gateway.Name,
                Location = gateway.Location,
                Latitude = gateway.Latitude,
                Longitude = gateway.Longitude,
                Status = gateway.Status,
                LastHeartbeat = gateway.LastHeartbeat,
                InstalledDate = gateway.InstalledDate,
                FirmwareVersion = gateway.FirmwareVersion,
                ChannelPlan = gateway.ChannelPlan,
                UplinkFrequency = gateway.UplinkFrequency,
                DownlinkFrequency = gateway.DownlinkFrequency,
                CoverageRadiusKm = gateway.CoverageRadiusKm,
                BackhaulType = gateway.BackhaulType,
                PacketForwardSuccessRate = gateway.PacketForwardSuccessRate,
                ConnectedDevices = connectedDevices
            };
        }

        private LoRaMeterReadingDto MapToMeterReadingDto(LoRaMeterReading reading)
        {
            var device = _devices.FirstOrDefault(d => d.Id == reading.DeviceId);

            return new LoRaMeterReadingDto
            {
                Id = reading.Id,
                DeviceId = reading.DeviceId,
                DeviceCode = device?.DeviceCode ?? string.Empty,
                DeviceName = device?.DeviceName ?? string.Empty,
                ReadingTime = reading.ReadingTime,
                Consumption = reading.Consumption,
                Temperature = reading.Temperature,
                BatteryLevel = reading.BatteryLevel,
                BatteryVoltage = reading.BatteryVoltage,
                Rssi = reading.Rssi,
                Snr = reading.Snr,
                PacketLossRate = reading.PacketLossRate,
                ValveStatus = reading.ValveStatus,
                TamperStatus = reading.TamperStatus,
                IsConfirmed = reading.IsConfirmed,
                DataRate = reading.DataRate
            };
        }

        private LoRaAlertDto MapToAlertDto(LoRaAlert alert)
        {
            var device = _devices.FirstOrDefault(d => d.Id == alert.DeviceId);

            return new LoRaAlertDto
            {
                Id = alert.Id,
                DeviceId = alert.DeviceId,
                DeviceCode = device?.DeviceCode ?? string.Empty,
                DeviceName = device?.DeviceName ?? string.Empty,
                AlertType = alert.AlertType,
                Severity = alert.Severity,
                Title = alert.Title,
                Message = alert.Message,
                CreatedAt = alert.CreatedAt,
                Status = alert.Status,
                AcknowledgedAt = alert.AcknowledgedAt,
                AcknowledgedBy = alert.AcknowledgedBy,
                ResolvedAt = alert.ResolvedAt,
                ResolvedBy = alert.ResolvedBy,
                ResolutionNotes = alert.ResolutionNotes
            };
        }
    }
}
