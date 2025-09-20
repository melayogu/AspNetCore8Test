namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class LoRaDeviceDto
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
        public string Status { get; set; } = string.Empty;
        public DateTime InstallDate { get; set; }
        public DateTime LastCommunication { get; set; }
        public int? GatewayId { get; set; }
        public string? GatewayName { get; set; }
        public int TransmissionIntervalMinutes { get; set; }
        public bool SupportsValveControl { get; set; }
        public bool SupportsTwoWayCommunication { get; set; }
        public decimal? LastReadingValue { get; set; }
        public DateTime? LastReadingAt { get; set; }
        public string? Notes { get; set; }
    }
}
