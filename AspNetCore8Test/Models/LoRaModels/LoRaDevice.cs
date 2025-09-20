namespace AspNetCore8Test.Models.LoRaModels
{
    public class LoRaDevice
    {
        public int Id { get; set; }
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal BatteryLevel { get; set; }
        public int SignalStrength { get; set; }
        public decimal Snr { get; set; }
        public string FirmwareVersion { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public DateTime InstallDate { get; set; } = DateTime.UtcNow.Date;
        public DateTime LastCommunication { get; set; } = DateTime.UtcNow;
        public int? GatewayId { get; set; }
        public int TransmissionIntervalMinutes { get; set; }
        public bool SupportsValveControl { get; set; }
        public bool SupportsTwoWayCommunication { get; set; }
        public decimal? LastReadingValue { get; set; }
        public DateTime? LastReadingAt { get; set; }
        public string? Notes { get; set; }
    }
}
