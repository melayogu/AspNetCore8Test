namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class UpdateLoRaDeviceDto
    {
        public string DeviceName { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? GatewayId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public decimal BatteryLevel { get; set; }
        public int SignalStrength { get; set; }
        public decimal Snr { get; set; }
        public int TransmissionIntervalMinutes { get; set; }
        public bool SupportsValveControl { get; set; }
        public bool SupportsTwoWayCommunication { get; set; }
        public string? Notes { get; set; }
    }
}
