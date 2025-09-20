using AspNetCore8Test.Models.DTOs.LoRaDTOs;
using AspNetCore8Test.Models.LoRaModels;

namespace AspNetCore8Test.Services.LoRaServices
{
    public interface ILoRaAlertService
    {
        Task<IEnumerable<LoRaAlertDto>> GetAllAlertsAsync();
        Task<IEnumerable<LoRaAlertDto>> GetActiveAlertsAsync();
        Task<IEnumerable<LoRaAlertDto>> GetAlertsByDeviceAsync(string deviceEui);
        Task<LoRaAlertDto?> GetAlertByIdAsync(int id);
        Task<LoRaAlertDto> CreateAlertAsync(CreateLoRaAlertDto createDto);
        Task<LoRaAlertDto?> AcknowledgeAlertAsync(int id, AcknowledgeLoRaAlertDto acknowledgeDto);
    }

    public class LoRaAlertService : ILoRaAlertService
    {
        private static readonly List<LoRaAlert> _alerts = new()
        {
            new LoRaAlert
            {
                Id = 1,
                DeviceEui = "ABCDEF1234567890",
                GatewayEui = "1122334455667788",
                AlertType = "BatteryLow",
                Severity = "High",
                Message = "設備電量低於 20%",
                SuggestedAction = "安排現場人員更換電池",
                OccurredAt = DateTime.UtcNow.AddHours(-4),
                IsAcknowledged = false
            },
            new LoRaAlert
            {
                Id = 2,
                DeviceEui = "A1B2C3D4E5F60789",
                GatewayEui = "8899AABBCCDDEEFF",
                AlertType = "CommunicationLost",
                Severity = "Critical",
                Message = "超過 2 小時未收到上行封包",
                SuggestedAction = "確認閘道器狀態與現場覆蓋品質",
                OccurredAt = DateTime.UtcNow.AddHours(-5),
                IsAcknowledged = true,
                AcknowledgedAt = DateTime.UtcNow.AddHours(-3)
            }
        };

        private static int _nextId = 3;

        public async Task<IEnumerable<LoRaAlertDto>> GetAllAlertsAsync()
        {
            await Task.Delay(1);
            return _alerts
                .OrderByDescending(a => a.OccurredAt)
                .Select(MapToDto);
        }

        public async Task<IEnumerable<LoRaAlertDto>> GetActiveAlertsAsync()
        {
            await Task.Delay(1);
            return _alerts
                .Where(a => !a.IsAcknowledged)
                .OrderByDescending(a => a.OccurredAt)
                .Select(MapToDto);
        }

        public async Task<IEnumerable<LoRaAlertDto>> GetAlertsByDeviceAsync(string deviceEui)
        {
            await Task.Delay(1);
            return _alerts
                .Where(a => a.DeviceEui.Equals(deviceEui, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.OccurredAt)
                .Select(MapToDto);
        }

        public async Task<LoRaAlertDto?> GetAlertByIdAsync(int id)
        {
            await Task.Delay(1);
            var alert = _alerts.FirstOrDefault(a => a.Id == id);
            return alert != null ? MapToDto(alert) : null;
        }

        public async Task<LoRaAlertDto> CreateAlertAsync(CreateLoRaAlertDto createDto)
        {
            await Task.Delay(1);
            var alert = new LoRaAlert
            {
                Id = _nextId++,
                DeviceEui = createDto.DeviceEui,
                GatewayEui = createDto.GatewayEui,
                AlertType = createDto.AlertType,
                Severity = createDto.Severity,
                Message = createDto.Message,
                SuggestedAction = string.IsNullOrWhiteSpace(createDto.SuggestedAction)
                    ? GetDefaultSuggestedAction(createDto.AlertType)
                    : createDto.SuggestedAction,
                OccurredAt = createDto.OccurredAt,
                IsAcknowledged = false
            };

            _alerts.Add(alert);
            return MapToDto(alert);
        }

        public async Task<LoRaAlertDto?> AcknowledgeAlertAsync(int id, AcknowledgeLoRaAlertDto acknowledgeDto)
        {
            await Task.Delay(1);
            var alert = _alerts.FirstOrDefault(a => a.Id == id);
            if (alert == null)
            {
                return null;
            }

            alert.IsAcknowledged = acknowledgeDto.IsAcknowledged;
            alert.AcknowledgedAt = acknowledgeDto.IsAcknowledged
                ? acknowledgeDto.AcknowledgedAt ?? DateTime.UtcNow
                : null;

            if (!string.IsNullOrWhiteSpace(acknowledgeDto.SuggestedAction))
            {
                alert.SuggestedAction = acknowledgeDto.SuggestedAction!;
            }

            return MapToDto(alert);
        }

        private static string GetDefaultSuggestedAction(string alertType)
        {
            return alertType switch
            {
                "BatteryLow" => "安排人員現場檢測並更換電池",
                "CommunicationLost" => "檢查閘道器、天線與微電腦裝置連線狀態",
                "Tamper" => "確認現場裝置是否遭破壞並比對讀值",
                _ => "派遣現場人員檢查設備"
            };
        }

        private static LoRaAlertDto MapToDto(LoRaAlert alert)
        {
            return new LoRaAlertDto
            {
                Id = alert.Id,
                DeviceEui = alert.DeviceEui,
                GatewayEui = alert.GatewayEui,
                AlertType = alert.AlertType,
                Severity = alert.Severity,
                Message = alert.Message,
                SuggestedAction = alert.SuggestedAction,
                OccurredAt = alert.OccurredAt,
                IsAcknowledged = alert.IsAcknowledged,
                AcknowledgedAt = alert.AcknowledgedAt
            };
        }
    }
}
