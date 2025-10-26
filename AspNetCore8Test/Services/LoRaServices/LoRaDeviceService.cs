using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface ILoRaDeviceService
    {
        Task<IEnumerable<LoRaDeviceDto>> GetAllDevicesAsync();
        Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id);
        Task<LoRaDeviceDto?> GetDeviceByEuiAsync(string deviceEui);
        Task<IEnumerable<LoRaDeviceDto>> GetDevicesByGatewayAsync(string gatewayEui);
        Task<IEnumerable<LoRaDeviceDto>> GetDevicesByStatusAsync(string status);
        Task<IEnumerable<LoRaDeviceDto>> SearchDevicesAsync(string searchTerm);
        Task<LoRaDeviceDto> CreateDeviceAsync(CreateLoRaDeviceDto createDto);
        Task<LoRaDeviceDto?> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto updateDto);
        Task<LoRaDeviceDto?> UpdateDeviceStatusAsync(string deviceEui, UpdateLoRaDeviceStatusDto statusDto);
        Task<bool> DeleteDeviceAsync(int id);
        Task<bool> DeviceExistsAsync(int id);
        Task<bool> DeviceEuiExistsAsync(string deviceEui);
    }

    public class LoRaDeviceService : ILoRaDeviceService
    {
        private static readonly List<LoRaDevice> _devices = new()
        {
            new LoRaDevice
            {
                Id = 1,
                DeviceEui = "ABCDEF1234567890",
                Name = "A1 棟地下一樓水錶",
                MeterType = "Water",
                FirmwareVersion = "1.2.0",
                BatteryLevel = 78,
                InstallLocation = "台北市信義區松智路1號 B1",
                InstallDate = DateTime.UtcNow.AddMonths(-18),
                LastCommunication = DateTime.UtcNow.AddMinutes(-15),
                IsActive = true,
                Status = "Active",
                GatewayEui = "1122334455667788",
                ApplicationKey = "00112233445566778899AABBCCDDEEFF",
                FrequencyPlan = "AS923",
                AlertThreshold = 5,
                SupportsClassC = false,
                LastSignalStrength = -112,
                LastReadingValue = 1280.5m,
                Notes = "定期於每季保養"
            },
            new LoRaDevice
            {
                Id = 2,
                DeviceEui = "1234567890ABCDEF",
                Name = "A1 棟一樓電錶",
                MeterType = "Electricity",
                FirmwareVersion = "2.0.1",
                BatteryLevel = 95,
                InstallLocation = "台北市信義區松智路1號 1F",
                InstallDate = DateTime.UtcNow.AddMonths(-10),
                LastCommunication = DateTime.UtcNow.AddMinutes(-5),
                IsActive = true,
                Status = "Active",
                GatewayEui = "1122334455667788",
                ApplicationKey = "FFEEDDCCBBAA99887766554433221100",
                FrequencyPlan = "AS923",
                AlertThreshold = 10,
                SupportsClassC = true,
                LastSignalStrength = -105,
                LastReadingValue = 8650.2m,
                Notes = "支援 OTA 升級"
            },
            new LoRaDevice
            {
                Id = 3,
                DeviceEui = "A1B2C3D4E5F60789",
                Name = "A2 棟天然氣錶",
                MeterType = "Gas",
                FirmwareVersion = "1.1.4",
                BatteryLevel = 62,
                InstallLocation = "新北市板橋區文化路 2 段 100 號",
                InstallDate = DateTime.UtcNow.AddMonths(-24),
                LastCommunication = DateTime.UtcNow.AddHours(-2),
                IsActive = true,
                Status = "Maintenance",
                GatewayEui = "8899AABBCCDDEEFF",
                ApplicationKey = "A1B2C3D4E5F60718293A4B5C6D7E8F90",
                FrequencyPlan = "AS923",
                AlertThreshold = 8,
                SupportsClassC = false,
                LastSignalStrength = -120,
                LastReadingValue = 452.75m,
                Notes = "排程更換電池"
            }
        };

        private static int _nextId = 4;

        public async Task<IEnumerable<LoRaDeviceDto>> GetAllDevicesAsync()
        {
            await Task.Delay(1);
            return _devices
                .OrderByDescending(d => d.LastCommunication)
                .Select(MapToDto);
        }

        public async Task<LoRaDeviceDto?> GetDeviceByIdAsync(int id)
        {
            await Task.Delay(1);
            var device = _devices.FirstOrDefault(d => d.Id == id);
            return device != null ? MapToDto(device) : null;
        }

        public async Task<LoRaDeviceDto?> GetDeviceByEuiAsync(string deviceEui)
        {
            await Task.Delay(1);
            var device = _devices.FirstOrDefault(d => d.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));
            return device != null ? MapToDto(device) : null;
        }

        public async Task<IEnumerable<LoRaDeviceDto>> GetDevicesByGatewayAsync(string gatewayEui)
        {
            await Task.Delay(1);
            return _devices
                .Where(d => d.GatewayEui.Equals(gatewayEui, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        public async Task<IEnumerable<LoRaDeviceDto>> GetDevicesByStatusAsync(string status)
        {
            await Task.Delay(1);
            return _devices
                .Where(d => d.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        public async Task<IEnumerable<LoRaDeviceDto>> SearchDevicesAsync(string searchTerm)
        {
            await Task.Delay(1);
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllDevicesAsync();
            }

            searchTerm = searchTerm.Trim();
            return _devices
                .Where(d =>
                    d.DeviceEui.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.MeterType.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.InstallLocation.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        public async Task<LoRaDeviceDto> CreateDeviceAsync(CreateLoRaDeviceDto createDto)
        {
            await Task.Delay(1);
            var device = new LoRaDevice
            {
                Id = _nextId++,
                DeviceEui = createDto.DeviceEui,
                Name = createDto.Name,
                MeterType = createDto.MeterType,
                FirmwareVersion = createDto.FirmwareVersion,
                BatteryLevel = createDto.BatteryLevel,
                InstallLocation = createDto.InstallLocation,
                InstallDate = createDto.InstallDate,
                LastCommunication = DateTime.UtcNow,
                IsActive = true,
                Status = "Active",
                GatewayEui = createDto.GatewayEui,
                ApplicationKey = createDto.ApplicationKey,
                FrequencyPlan = createDto.FrequencyPlan,
                AlertThreshold = createDto.AlertThreshold,
                SupportsClassC = createDto.SupportsClassC,
                LastSignalStrength = -130,
                LastReadingValue = 0,
                Notes = createDto.Notes
            };

            _devices.Add(device);
            return MapToDto(device);
        }

        public async Task<LoRaDeviceDto?> UpdateDeviceAsync(int id, UpdateLoRaDeviceDto updateDto)
        {
            await Task.Delay(1);
            var device = _devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
            {
                return null;
            }

            device.DeviceEui = updateDto.DeviceEui;
            device.Name = updateDto.Name;
            device.MeterType = updateDto.MeterType;
            device.FirmwareVersion = updateDto.FirmwareVersion;
            device.BatteryLevel = updateDto.BatteryLevel;
            device.InstallLocation = updateDto.InstallLocation;
            device.InstallDate = updateDto.InstallDate;
            device.GatewayEui = updateDto.GatewayEui;
            device.ApplicationKey = updateDto.ApplicationKey;
            device.FrequencyPlan = updateDto.FrequencyPlan;
            device.AlertThreshold = updateDto.AlertThreshold;
            device.SupportsClassC = updateDto.SupportsClassC;
            device.IsActive = updateDto.IsActive;
            device.Status = updateDto.Status;
            device.LastSignalStrength = updateDto.LastSignalStrength;
            device.Notes = updateDto.Notes;

            return MapToDto(device);
        }

        public async Task<LoRaDeviceDto?> UpdateDeviceStatusAsync(string deviceEui, UpdateLoRaDeviceStatusDto statusDto)
        {
            await Task.Delay(1);
            var device = _devices.FirstOrDefault(d => d.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));
            if (device == null)
            {
                return null;
            }

            device.Status = statusDto.Status;
            device.LastCommunication = DateTime.UtcNow;

            if (statusDto.BatteryLevel.HasValue)
            {
                device.BatteryLevel = statusDto.BatteryLevel.Value;
            }

            if (statusDto.IsActive.HasValue)
            {
                device.IsActive = statusDto.IsActive.Value;
            }

            if (!string.IsNullOrWhiteSpace(statusDto.GatewayEui))
            {
                device.GatewayEui = statusDto.GatewayEui!;
            }

            if (statusDto.LastSignalStrength.HasValue)
            {
                device.LastSignalStrength = statusDto.LastSignalStrength.Value;
            }

            return MapToDto(device);
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            await Task.Delay(1);
            var device = _devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
            {
                return false;
            }

            _devices.Remove(device);
            return true;
        }

        public async Task<bool> DeviceExistsAsync(int id)
        {
            await Task.Delay(1);
            return _devices.Any(d => d.Id == id);
        }

        public async Task<bool> DeviceEuiExistsAsync(string deviceEui)
        {
            await Task.Delay(1);
            return _devices.Any(d => d.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase));
        }

        private static LoRaDeviceDto MapToDto(LoRaDevice device)
        {
            return new LoRaDeviceDto
            {
                Id = device.Id,
                DeviceEui = device.DeviceEui,
                Name = device.Name,
                MeterType = device.MeterType,
                FirmwareVersion = device.FirmwareVersion,
                BatteryLevel = device.BatteryLevel,
                InstallLocation = device.InstallLocation,
                InstallDate = device.InstallDate,
                LastCommunication = device.LastCommunication,
                IsActive = device.IsActive,
                Status = device.Status,
                GatewayEui = device.GatewayEui,
                ApplicationKey = device.ApplicationKey,
                FrequencyPlan = device.FrequencyPlan,
                AlertThreshold = device.AlertThreshold,
                SupportsClassC = device.SupportsClassC,
                LastSignalStrength = device.LastSignalStrength,
                LastReadingValue = device.LastReadingValue,
                Notes = device.Notes
            };
        }
    }
}
