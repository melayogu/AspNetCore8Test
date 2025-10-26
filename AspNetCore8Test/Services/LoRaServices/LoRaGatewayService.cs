using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface ILoRaGatewayService
    {
        Task<IEnumerable<LoRaGatewayDto>> GetAllGatewaysAsync();
        Task<LoRaGatewayDto?> GetGatewayByIdAsync(int id);
        Task<LoRaGatewayDto?> GetGatewayByEuiAsync(string gatewayEui);
        Task<IEnumerable<LoRaGatewayDto>> SearchGatewaysAsync(string searchTerm);
        Task<LoRaGatewayDto> CreateGatewayAsync(CreateLoRaGatewayDto createDto);
        Task<LoRaGatewayDto?> UpdateGatewayAsync(int id, UpdateLoRaGatewayDto updateDto);
        Task<LoRaGatewayDto?> UpdateGatewayStatusAsync(string gatewayEui, UpdateLoRaGatewayStatusDto statusDto);
        Task<bool> DeleteGatewayAsync(int id);
        Task<bool> GatewayExistsAsync(int id);
        Task<bool> GatewayEuiExistsAsync(string gatewayEui);
    }

    public class LoRaGatewayService : ILoRaGatewayService
    {
        private static readonly List<LoRaGateway> _gateways = new()
        {
            new LoRaGateway
            {
                Id = 1,
                GatewayEui = "1122334455667788",
                Name = "信義行政中心閘道器",
                Location = "台北市信義區松智路 1 號 15F",
                IpAddress = "10.10.0.10",
                FirmwareVersion = "3.1.0",
                IsOnline = true,
                LastCommunication = DateTime.UtcNow.AddMinutes(-2),
                Latitude = 25.0330,
                Longitude = 121.5654,
                Status = "Normal",
                ConnectedDevices = 24,
                PacketSuccessRate = 98.5,
                BackhaulType = "Fiber",
                Notes = "主幹節點，與 NMS 每 5 分鐘同步"
            },
            new LoRaGateway
            {
                Id = 2,
                GatewayEui = "8899AABBCCDDEEFF",
                Name = "板橋分局閘道器",
                Location = "新北市板橋區文化路 2 段 100 號",
                IpAddress = "10.20.0.5",
                FirmwareVersion = "3.0.2",
                IsOnline = false,
                LastCommunication = DateTime.UtcNow.AddHours(-3),
                Latitude = 25.0145,
                Longitude = 121.4670,
                Status = "Maintenance",
                ConnectedDevices = 12,
                PacketSuccessRate = 92.3,
                BackhaulType = "Cellular",
                Notes = "預計今晚 23:00 更新韌體"
            }
        };

        private static int _nextId = 3;

        public async Task<IEnumerable<LoRaGatewayDto>> GetAllGatewaysAsync()
        {
            await Task.Delay(1);
            return _gateways
                .OrderByDescending(g => g.LastCommunication)
                .Select(MapToDto);
        }

        public async Task<LoRaGatewayDto?> GetGatewayByIdAsync(int id)
        {
            await Task.Delay(1);
            var gateway = _gateways.FirstOrDefault(g => g.Id == id);
            return gateway != null ? MapToDto(gateway) : null;
        }

        public async Task<LoRaGatewayDto?> GetGatewayByEuiAsync(string gatewayEui)
        {
            await Task.Delay(1);
            var gateway = _gateways.FirstOrDefault(g => g.GatewayEui.Equals(gatewayEui, StringComparison.OrdinalIgnoreCase));
            return gateway != null ? MapToDto(gateway) : null;
        }

        public async Task<IEnumerable<LoRaGatewayDto>> SearchGatewaysAsync(string searchTerm)
        {
            await Task.Delay(1);
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllGatewaysAsync();
            }

            searchTerm = searchTerm.Trim();
            return _gateways
                .Where(g =>
                    g.GatewayEui.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto);
        }

        public async Task<LoRaGatewayDto> CreateGatewayAsync(CreateLoRaGatewayDto createDto)
        {
            await Task.Delay(1);
            var gateway = new LoRaGateway
            {
                Id = _nextId++,
                GatewayEui = createDto.GatewayEui,
                Name = createDto.Name,
                Location = createDto.Location,
                IpAddress = createDto.IpAddress,
                FirmwareVersion = createDto.FirmwareVersion,
                IsOnline = true,
                LastCommunication = DateTime.UtcNow,
                Latitude = createDto.Latitude,
                Longitude = createDto.Longitude,
                Status = "Normal",
                ConnectedDevices = 0,
                PacketSuccessRate = createDto.PacketSuccessRate,
                BackhaulType = createDto.BackhaulType,
                Notes = createDto.Notes
            };

            _gateways.Add(gateway);
            return MapToDto(gateway);
        }

        public async Task<LoRaGatewayDto?> UpdateGatewayAsync(int id, UpdateLoRaGatewayDto updateDto)
        {
            await Task.Delay(1);
            var gateway = _gateways.FirstOrDefault(g => g.Id == id);
            if (gateway == null)
            {
                return null;
            }

            gateway.GatewayEui = updateDto.GatewayEui;
            gateway.Name = updateDto.Name;
            gateway.Location = updateDto.Location;
            gateway.IpAddress = updateDto.IpAddress;
            gateway.FirmwareVersion = updateDto.FirmwareVersion;
            gateway.IsOnline = updateDto.IsOnline;
            gateway.LastCommunication = DateTime.UtcNow;
            gateway.Latitude = updateDto.Latitude;
            gateway.Longitude = updateDto.Longitude;
            gateway.Status = updateDto.Status;
            gateway.ConnectedDevices = updateDto.ConnectedDevices;
            gateway.PacketSuccessRate = updateDto.PacketSuccessRate;
            gateway.BackhaulType = updateDto.BackhaulType;
            gateway.Notes = updateDto.Notes;

            return MapToDto(gateway);
        }

        public async Task<LoRaGatewayDto?> UpdateGatewayStatusAsync(string gatewayEui, UpdateLoRaGatewayStatusDto statusDto)
        {
            await Task.Delay(1);
            var gateway = _gateways.FirstOrDefault(g => g.GatewayEui.Equals(gatewayEui, StringComparison.OrdinalIgnoreCase));
            if (gateway == null)
            {
                return null;
            }

            gateway.IsOnline = statusDto.IsOnline;
            gateway.LastCommunication = DateTime.UtcNow;

            if (statusDto.PacketSuccessRate.HasValue)
            {
                gateway.PacketSuccessRate = statusDto.PacketSuccessRate.Value;
            }

            if (statusDto.ConnectedDevices.HasValue)
            {
                gateway.ConnectedDevices = statusDto.ConnectedDevices.Value;
            }

            if (!string.IsNullOrWhiteSpace(statusDto.Status))
            {
                gateway.Status = statusDto.Status!;
            }

            return MapToDto(gateway);
        }

        public async Task<bool> DeleteGatewayAsync(int id)
        {
            await Task.Delay(1);
            var gateway = _gateways.FirstOrDefault(g => g.Id == id);
            if (gateway == null)
            {
                return false;
            }

            _gateways.Remove(gateway);
            return true;
        }

        public async Task<bool> GatewayExistsAsync(int id)
        {
            await Task.Delay(1);
            return _gateways.Any(g => g.Id == id);
        }

        public async Task<bool> GatewayEuiExistsAsync(string gatewayEui)
        {
            await Task.Delay(1);
            return _gateways.Any(g => g.GatewayEui.Equals(gatewayEui, StringComparison.OrdinalIgnoreCase));
        }

        private static LoRaGatewayDto MapToDto(LoRaGateway gateway)
        {
            return new LoRaGatewayDto
            {
                Id = gateway.Id,
                GatewayEui = gateway.GatewayEui,
                Name = gateway.Name,
                Location = gateway.Location,
                IpAddress = gateway.IpAddress,
                FirmwareVersion = gateway.FirmwareVersion,
                IsOnline = gateway.IsOnline,
                LastCommunication = gateway.LastCommunication,
                Latitude = gateway.Latitude,
                Longitude = gateway.Longitude,
                Status = gateway.Status,
                ConnectedDevices = gateway.ConnectedDevices,
                PacketSuccessRate = gateway.PacketSuccessRate,
                BackhaulType = gateway.BackhaulType,
                Notes = gateway.Notes
            };
        }
    }
}
