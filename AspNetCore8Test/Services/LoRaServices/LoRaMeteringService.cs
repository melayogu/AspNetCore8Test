using AspNetCore8Test.Models.DTOs.LoRaDtos;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface ILoRaMeteringService
    {
        Task<IEnumerable<LoRaDeviceDto>> GetAllDevicesAsync();
        Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id);
        Task<LoRaDeviceDto?> GetDeviceByEuiAsync(string deviceEui);
        Task<LoRaDeviceDto?> GetDeviceByMeterNumberAsync(string meterNumber);
        Task<LoRaDeviceDto> CreateDeviceAsync(CreateLoRaDeviceDto dto);
        Task<LoRaDeviceDto?> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto dto);
        Task<bool> DeleteDeviceAsync(int id);
        Task<IEnumerable<MeterReadingDto>> GetDeviceReadingsAsync(int deviceId, DateTime? startDate = null, DateTime? endDate = null);
        Task<MeterReadingDto?> AddDeviceReadingAsync(int deviceId, CreateMeterReadingDto dto);
        Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync();
        Task<LoRaNetworkSummaryDto> GetNetworkSummaryAsync();
        Task<IEnumerable<LoRaAlertDto>> GetActiveAlertsAsync();
        Task<IEnumerable<LoRaAlertDto>> GetDeviceAlertsAsync(int deviceId);
        Task<LoRaAlertDto?> AcknowledgeAlertAsync(int alertId, string operatorName);
        Task<LoRaAlertDto?> ResolveAlertAsync(int alertId, string operatorName);
    }

    public class LoRaMeteringService : ILoRaMeteringService
    {
        private static readonly List<LoRaGateway> Gateways = new()
        {
            new LoRaGateway
            {
                Id = 1,
                GatewayEui = "A1B2C3D4E5F6A7B8",
                Name = "北區智慧閘道",
                Location = "台北市大安區復興南路二段 300 號",
                Latitude = 25.0330,
                Longitude = 121.5438,
                FirmwareVersion = "1.4.2",
                BackhaulType = "4G",
                FrequencyPlan = "AS923",
                LastHeartbeatAt = DateTime.UtcNow.AddMinutes(-2),
                IsOnline = true,
                ConnectedDevices = 48
            },
            new LoRaGateway
            {
                Id = 2,
                GatewayEui = "B1C2D3E4F5A6B7C8",
                Name = "南區韌性閘道",
                Location = "高雄市苓雅區成功一路 120 號",
                Latitude = 22.6200,
                Longitude = 120.3000,
                FirmwareVersion = "1.3.9",
                BackhaulType = "Fiber",
                FrequencyPlan = "AS923",
                LastHeartbeatAt = DateTime.UtcNow.AddMinutes(-17),
                IsOnline = false,
                ConnectedDevices = 32
            }
        };

        private static readonly List<LoRaDevice> Devices = new()
        {
            new LoRaDevice
            {
                Id = 1,
                DeviceEui = "ABCDEF1234567890",
                MeterNumber = "MTR-0001",
                MeterType = "智慧水表",
                FirmwareVersion = "2.1.0",
                GatewayId = 1,
                InstallationLocation = "台北市信義區松高路 16 號",
                InstallationNotes = "地下室水錶間，需防潮",
                InstalledAt = DateTime.UtcNow.AddMonths(-8),
                IsActive = true,
                LastCommunicationAt = DateTime.UtcNow.AddMinutes(-5),
                LastReadingValue = 1286.4,
                LastReadingUnit = "m3",
                BatteryLevel = 86.5,
                SignalStrength = -95,
                SignalToNoiseRatio = 8.7,
                TamperDetected = false,
                Status = "Connected"
            },
            new LoRaDevice
            {
                Id = 2,
                DeviceEui = "1234567890ABCDEF",
                MeterNumber = "MTR-0002",
                MeterType = "智慧瓦斯表",
                FirmwareVersion = "2.0.5",
                GatewayId = 1,
                InstallationLocation = "新北市板橋區文化路一段 250 號",
                InstallationNotes = "戶外陽台，需要定期檢查防水",
                InstalledAt = DateTime.UtcNow.AddMonths(-12),
                IsActive = true,
                LastCommunicationAt = DateTime.UtcNow.AddMinutes(-18),
                LastReadingValue = 864.2,
                LastReadingUnit = "m3",
                BatteryLevel = 73.2,
                SignalStrength = -103,
                SignalToNoiseRatio = 6.5,
                TamperDetected = false,
                Status = "Connected"
            },
            new LoRaDevice
            {
                Id = 3,
                DeviceEui = "F1E2D3C4B5A69788",
                MeterNumber = "MTR-0003",
                MeterType = "智慧電表",
                FirmwareVersion = "3.0.1",
                GatewayId = 2,
                InstallationLocation = "高雄市鼓山區美術東二路 55 號",
                InstallationNotes = "設備室第三排",
                InstalledAt = DateTime.UtcNow.AddMonths(-3),
                IsActive = false,
                LastCommunicationAt = DateTime.UtcNow.AddHours(-4),
                LastReadingValue = 15234.78,
                LastReadingUnit = "kWh",
                BatteryLevel = 49.4,
                SignalStrength = -120,
                SignalToNoiseRatio = 2.1,
                TamperDetected = true,
                Status = "Maintenance"
            }
        };

        private static readonly List<MeterReading> Readings = new()
        {
            new MeterReading
            {
                Id = 1,
                DeviceId = 1,
                Timestamp = DateTime.UtcNow.AddHours(-24),
                Value = 1278.9,
                Unit = "m3",
                SignalStrength = -97,
                SignalToNoiseRatio = 7.9,
                BatteryLevel = 88.2,
                TransmissionStatus = "Success",
                FrameCounter = 1024
            },
            new MeterReading
            {
                Id = 2,
                DeviceId = 1,
                Timestamp = DateTime.UtcNow.AddHours(-2),
                Value = 1286.4,
                Unit = "m3",
                SignalStrength = -95,
                SignalToNoiseRatio = 8.7,
                BatteryLevel = 86.5,
                TransmissionStatus = "Success",
                FrameCounter = 1128
            },
            new MeterReading
            {
                Id = 3,
                DeviceId = 2,
                Timestamp = DateTime.UtcNow.AddHours(-1),
                Value = 864.2,
                Unit = "m3",
                SignalStrength = -103,
                SignalToNoiseRatio = 6.5,
                BatteryLevel = 73.2,
                TransmissionStatus = "Success",
                FrameCounter = 1456
            },
            new MeterReading
            {
                Id = 4,
                DeviceId = 3,
                Timestamp = DateTime.UtcNow.AddHours(-5),
                Value = 15234.78,
                Unit = "kWh",
                SignalStrength = -120,
                SignalToNoiseRatio = 2.1,
                BatteryLevel = 49.4,
                TransmissionStatus = "Warning",
                FrameCounter = 210
            }
        };

        private static readonly List<LoRaAlert> Alerts = new()
        {
            new LoRaAlert
            {
                Id = 1,
                DeviceId = 3,
                DeviceEui = "F1E2D3C4B5A69788",
                Severity = "High",
                Category = "Tamper",
                Message = "偵測到磁干擾，請派員現場確認",
                CreatedAt = DateTime.UtcNow.AddHours(-3),
                IsResolved = false
            },
            new LoRaAlert
            {
                Id = 2,
                DeviceId = 2,
                DeviceEui = "1234567890ABCDEF",
                Severity = "Medium",
                Category = "Battery",
                Message = "電池電量低於 75%",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                AcknowledgedAt = DateTime.UtcNow.AddHours(-20),
                AcknowledgedBy = "系統自動", 
                IsResolved = true,
                ResolvedAt = DateTime.UtcNow.AddHours(-19),
                ResolvedBy = "系統自動"
            }
        };

        private static int _nextDeviceId = Devices.Any() ? Devices.Max(d => d.Id) + 1 : 1;
        private static int _nextReadingId = Readings.Any() ? Readings.Max(r => r.Id) + 1 : 1;

        public async Task<IEnumerable<LoRaDeviceDto>> GetAllDevicesAsync()
        {
            await Task.Delay(1);
            return Devices.Select(MapDeviceToDto).ToList();
        }

        public async Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id)
        {
            await Task.Delay(1);
            var device = Devices.FirstOrDefault(d => d.Id == id);
            return device == null ? null : MapDeviceToDto(device);
        }

        public async Task<LoRaDeviceDto?> GetDeviceByEuiAsync(string deviceEui)
        {
            await Task.Delay(1);
            var device = Devices.FirstOrDefault(d => d.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));
            return device == null ? null : MapDeviceToDto(device);
        }

        public async Task<LoRaDeviceDto?> GetDeviceByMeterNumberAsync(string meterNumber)
        {
            await Task.Delay(1);
            var device = Devices.FirstOrDefault(d => d.MeterNumber.Equals(meterNumber, StringComparison.OrdinalIgnoreCase));
            return device == null ? null : MapDeviceToDto(device);
        }

        public async Task<LoRaDeviceDto> CreateDeviceAsync(CreateLoRaDeviceDto dto)
        {
            await Task.Delay(1);

            var device = new LoRaDevice
            {
                Id = _nextDeviceId++,
                DeviceEui = dto.DeviceEui,
                MeterNumber = dto.MeterNumber,
                MeterType = dto.MeterType,
                FirmwareVersion = dto.FirmwareVersion,
                GatewayId = dto.GatewayId,
                InstallationLocation = dto.InstallationLocation,
                InstallationNotes = dto.InstallationNotes,
                InstalledAt = dto.InstalledAt,
                IsActive = true,
                Status = "Provisioning",
                LastCommunicationAt = dto.InstalledAt,
                BatteryLevel = 100,
                SignalStrength = -80,
                SignalToNoiseRatio = 10.0,
                LastReadingUnit = string.Empty,
                LastReadingValue = 0,
                TamperDetected = false
            };

            Devices.Add(device);
            UpdateGatewayConnectionCount(device.GatewayId);
            return MapDeviceToDto(device);
        }

        public async Task<LoRaDeviceDto?> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto dto)
        {
            await Task.Delay(1);

            var device = Devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
            {
                return null;
            }

            if (device.GatewayId != dto.GatewayId)
            {
                UpdateGatewayConnectionCount(device.GatewayId, decrement: true);
            }

            device.DeviceEui = dto.DeviceEui;
            device.MeterNumber = dto.MeterNumber;
            device.MeterType = dto.MeterType;
            device.FirmwareVersion = dto.FirmwareVersion;
            device.GatewayId = dto.GatewayId;
            device.InstallationLocation = dto.InstallationLocation;
            device.InstallationNotes = dto.InstallationNotes;
            device.InstalledAt = dto.InstalledAt;
            device.IsActive = dto.IsActive;
            device.Status = dto.Status;
            device.BatteryLevel = dto.BatteryLevel;
            device.SignalStrength = dto.SignalStrength;
            device.SignalToNoiseRatio = dto.SignalToNoiseRatio;
            device.TamperDetected = dto.TamperDetected;
            device.LastReadingValue = dto.LastReadingValue;
            device.LastReadingUnit = dto.LastReadingUnit;
            device.LastCommunicationAt = dto.LastCommunicationAt;

            UpdateGatewayConnectionCount(device.GatewayId);
            return MapDeviceToDto(device);
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            await Task.Delay(1);
            var device = Devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
            {
                return false;
            }

            Devices.Remove(device);
            Readings.RemoveAll(r => r.DeviceId == id);
            Alerts.RemoveAll(a => a.DeviceId == id);
            UpdateGatewayConnectionCount(device.GatewayId);
            return true;
        }

        public async Task<IEnumerable<MeterReadingDto>> GetDeviceReadingsAsync(int deviceId, DateTime? startDate = null, DateTime? endDate = null)
        {
            await Task.Delay(1);
            var query = Readings.Where(r => r.DeviceId == deviceId).AsEnumerable();

            if (startDate.HasValue)
            {
                query = query.Where(r => r.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.Timestamp <= endDate.Value);
            }

            return query
                .OrderByDescending(r => r.Timestamp)
                .Select(MapReadingToDto)
                .ToList();
        }

        public async Task<MeterReadingDto?> AddDeviceReadingAsync(int deviceId, CreateMeterReadingDto dto)
        {
            await Task.Delay(1);
            var device = Devices.FirstOrDefault(d => d.Id == deviceId);
            if (device == null)
            {
                return null;
            }

            var reading = new MeterReading
            {
                Id = _nextReadingId++,
                DeviceId = deviceId,
                Timestamp = dto.Timestamp,
                Value = dto.Value,
                Unit = dto.Unit,
                SignalStrength = dto.SignalStrength,
                SignalToNoiseRatio = dto.SignalToNoiseRatio,
                BatteryLevel = dto.BatteryLevel,
                TransmissionStatus = dto.TransmissionStatus,
                FrameCounter = Readings.Where(r => r.DeviceId == deviceId).Select(r => r.FrameCounter).DefaultIfEmpty().Max() + 1
            };

            Readings.Add(reading);

            device.LastReadingValue = dto.Value;
            device.LastReadingUnit = dto.Unit;
            device.LastCommunicationAt = dto.Timestamp;
            device.SignalStrength = dto.SignalStrength;
            device.SignalToNoiseRatio = dto.SignalToNoiseRatio;
            device.BatteryLevel = dto.BatteryLevel;
            device.Status = dto.TransmissionStatus.Equals("Success", StringComparison.OrdinalIgnoreCase)
                ? "Connected"
                : "Warning";

            return MapReadingToDto(reading);
        }

        public async Task<IEnumerable<LoRaGatewayDto>> GetGatewaysAsync()
        {
            await Task.Delay(1);
            foreach (var gateway in Gateways)
            {
                UpdateGatewayConnectionCount(gateway.Id);
            }

            return Gateways.Select(MapGatewayToDto).ToList();
        }

        public async Task<LoRaNetworkSummaryDto> GetNetworkSummaryAsync()
        {
            await Task.Delay(1);

            var totalDevices = Devices.Count;
            var activeDevices = Devices.Count(d => d.IsActive);
            var offlineDevices = Devices.Count(d => d.Status.Equals("Offline", StringComparison.OrdinalIgnoreCase));
            var alertingDevices = Alerts.Where(a => !a.IsResolved).Select(a => a.DeviceId).Distinct().Count();
            var averageBattery = totalDevices == 0 ? 0 : Math.Round(Devices.Average(d => d.BatteryLevel), 2);
            var averageRssi = totalDevices == 0 ? 0 : Math.Round(Devices.Average(d => d.SignalStrength), 2);
            var gatewaysOnline = Gateways.Count(g => g.IsOnline);
            var gatewaysOffline = Gateways.Count - gatewaysOnline;
            var readingLast24h = Readings.Count(r => r.Timestamp >= DateTime.UtcNow.AddHours(-24));
            var successCount = Readings.Count(r => r.TransmissionStatus.Equals("Success", StringComparison.OrdinalIgnoreCase));
            var successRate = Readings.Count == 0 ? 0 : Math.Round((double)successCount / Readings.Count * 100, 2);

            return new LoRaNetworkSummaryDto
            {
                TotalDevices = totalDevices,
                ActiveDevices = activeDevices,
                OfflineDevices = offlineDevices,
                AlertingDevices = alertingDevices,
                AverageBatteryLevel = averageBattery,
                AverageSignalStrength = averageRssi,
                GatewaysOnline = gatewaysOnline,
                GatewaysOffline = gatewaysOffline,
                ReadingsLast24Hours = readingLast24h,
                SuccessfulTransmissionRate = successRate,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<LoRaAlertDto>> GetActiveAlertsAsync()
        {
            await Task.Delay(1);
            return Alerts
                .Where(a => !a.IsResolved)
                .OrderByDescending(a => a.CreatedAt)
                .Select(MapAlertToDto)
                .ToList();
        }

        public async Task<IEnumerable<LoRaAlertDto>> GetDeviceAlertsAsync(int deviceId)
        {
            await Task.Delay(1);
            return Alerts
                .Where(a => a.DeviceId == deviceId)
                .OrderByDescending(a => a.CreatedAt)
                .Select(MapAlertToDto)
                .ToList();
        }

        public async Task<LoRaAlertDto?> AcknowledgeAlertAsync(int alertId, string operatorName)
        {
            await Task.Delay(1);
            var alert = Alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null)
            {
                return null;
            }

            alert.AcknowledgedAt = DateTime.UtcNow;
            alert.AcknowledgedBy = operatorName;
            return MapAlertToDto(alert);
        }

        public async Task<LoRaAlertDto?> ResolveAlertAsync(int alertId, string operatorName)
        {
            await Task.Delay(1);
            var alert = Alerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null)
            {
                return null;
            }

            alert.AcknowledgedAt ??= DateTime.UtcNow;
            alert.AcknowledgedBy ??= operatorName;
            alert.ResolvedAt = DateTime.UtcNow;
            alert.ResolvedBy = operatorName;
            alert.IsResolved = true;
            return MapAlertToDto(alert);
        }

        private static LoRaDeviceDto MapDeviceToDto(LoRaDevice device) => new()
        {
            Id = device.Id,
            DeviceEui = device.DeviceEui,
            MeterNumber = device.MeterNumber,
            MeterType = device.MeterType,
            FirmwareVersion = device.FirmwareVersion,
            GatewayId = device.GatewayId,
            InstallationLocation = device.InstallationLocation,
            InstallationNotes = device.InstallationNotes,
            InstalledAt = device.InstalledAt,
            IsActive = device.IsActive,
            LastCommunicationAt = device.LastCommunicationAt,
            LastReadingValue = device.LastReadingValue,
            LastReadingUnit = device.LastReadingUnit,
            BatteryLevel = device.BatteryLevel,
            SignalStrength = device.SignalStrength,
            SignalToNoiseRatio = device.SignalToNoiseRatio,
            TamperDetected = device.TamperDetected,
            Status = device.Status
        };

        private static LoRaGatewayDto MapGatewayToDto(LoRaGateway gateway) => new()
        {
            Id = gateway.Id,
            GatewayEui = gateway.GatewayEui,
            Name = gateway.Name,
            Location = gateway.Location,
            Latitude = gateway.Latitude,
            Longitude = gateway.Longitude,
            FirmwareVersion = gateway.FirmwareVersion,
            BackhaulType = gateway.BackhaulType,
            FrequencyPlan = gateway.FrequencyPlan,
            LastHeartbeatAt = gateway.LastHeartbeatAt,
            IsOnline = gateway.IsOnline,
            ConnectedDevices = gateway.ConnectedDevices
        };

        private static MeterReadingDto MapReadingToDto(MeterReading reading) => new()
        {
            Id = reading.Id,
            DeviceId = reading.DeviceId,
            Timestamp = reading.Timestamp,
            Value = reading.Value,
            Unit = reading.Unit,
            SignalStrength = reading.SignalStrength,
            SignalToNoiseRatio = reading.SignalToNoiseRatio,
            BatteryLevel = reading.BatteryLevel,
            TransmissionStatus = reading.TransmissionStatus,
            FrameCounter = reading.FrameCounter
        };

        private static LoRaAlertDto MapAlertToDto(LoRaAlert alert) => new()
        {
            Id = alert.Id,
            DeviceId = alert.DeviceId,
            DeviceEui = alert.DeviceEui,
            Severity = alert.Severity,
            Category = alert.Category,
            Message = alert.Message,
            CreatedAt = alert.CreatedAt,
            AcknowledgedAt = alert.AcknowledgedAt,
            AcknowledgedBy = alert.AcknowledgedBy,
            ResolvedAt = alert.ResolvedAt,
            ResolvedBy = alert.ResolvedBy,
            IsResolved = alert.IsResolved
        };

        private static void UpdateGatewayConnectionCount(int gatewayId, bool decrement = false)
        {
            var gateway = Gateways.FirstOrDefault(g => g.Id == gatewayId);
            if (gateway == null)
            {
                return;
            }

            if (decrement)
            {
                gateway.ConnectedDevices = Math.Max(0, gateway.ConnectedDevices - 1);
            }
            else
            {
                gateway.ConnectedDevices = Devices.Count(d => d.GatewayId == gatewayId && d.IsActive);
            }
        }
    }
}
