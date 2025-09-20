using System;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 智慧抄表裝置資訊
    /// </summary>
    public class LoRaDeviceDto
    {
        public int Id { get; set; }
        public string DeviceNumber { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double SignalStrength { get; set; }
        public double Snr { get; set; }
        public int BatteryLevel { get; set; }
        public bool IsBatteryCritical => BatteryLevel < 20;
        public string FirmwareVersion { get; set; } = string.Empty;
        public bool FirmwareUpToDate { get; set; }
        public DateTime LastReadingTime { get; set; }
        public decimal LastReadingValue { get; set; }
        public decimal MonthlyUsage { get; set; }
        public string GatewayId { get; set; } = string.Empty;
        public string AlertStatus { get; set; } = string.Empty;
        public bool HasRecentAlert { get; set; }
        public string InstallationType { get; set; } = string.Empty;
        public DateTime ActivatedAt { get; set; }
    }
}
