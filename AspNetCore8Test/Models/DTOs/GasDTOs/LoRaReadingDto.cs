using System;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 裝置的抄表傳輸紀錄
    /// </summary>
    public class LoRaReadingDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string DeviceNumber { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime ReadingTime { get; set; }
        public decimal ReadingValue { get; set; }
        public decimal Usage { get; set; }
        public double SignalStrength { get; set; }
        public double Snr { get; set; }
        public int BatteryLevel { get; set; }
        public bool IsAlert { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string TransmissionStatus { get; set; } = string.Empty;
        public string GatewayId { get; set; } = string.Empty;
        public double PacketLoss { get; set; }
    }
}
