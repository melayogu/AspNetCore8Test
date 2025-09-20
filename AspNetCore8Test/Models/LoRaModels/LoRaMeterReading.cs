namespace AspNetCore8Test.Models.LoRaModels
{
    public class LoRaMeterReading
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime ReadingTime { get; set; }
        public decimal Consumption { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? BatteryLevel { get; set; }
        public decimal? BatteryVoltage { get; set; }
        public int? Rssi { get; set; }
        public decimal? Snr { get; set; }
        public decimal? PacketLossRate { get; set; }
        public string? ValveStatus { get; set; }
        public string? TamperStatus { get; set; }
        public bool? IsConfirmed { get; set; }
        public string DataRate { get; set; } = string.Empty;
    }
}
