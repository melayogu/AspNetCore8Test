using System.Collections.Generic;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 裝置詳細資料頁的複合 ViewModel
    /// </summary>
    public class LoRaDeviceDetailsViewModel
    {
        public LoRaDeviceDto Device { get; set; } = new LoRaDeviceDto();
        public IEnumerable<LoRaReadingDto> RecentReadings { get; set; } = new List<LoRaReadingDto>();
        public IEnumerable<LoRaAlertDto> Alerts { get; set; } = new List<LoRaAlertDto>();
    }
}
