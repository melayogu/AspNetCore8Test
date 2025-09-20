using System.Collections.Generic;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 儀表板使用的複合資料模型
    /// </summary>
    public class LoRaDashboardViewModel
    {
        public LoRaOverviewDto Overview { get; set; } = new LoRaOverviewDto();
        public IEnumerable<LoRaDeviceDto> TopDevices { get; set; } = new List<LoRaDeviceDto>();
        public IEnumerable<LoRaReadingDto> RecentReadings { get; set; } = new List<LoRaReadingDto>();
        public IEnumerable<LoRaAlertDto> ActiveAlerts { get; set; } = new List<LoRaAlertDto>();
        public IEnumerable<LoRaNetworkStatusDto> NetworkStatus { get; set; } = new List<LoRaNetworkStatusDto>();
        public IEnumerable<LoRaGatewayDto> Gateways { get; set; } = new List<LoRaGatewayDto>();
    }
}
