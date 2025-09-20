namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateLoRaDeviceDto
    {
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? GatewayId { get; set; }
        public string? Status { get; set; }
        public DateTime? InstallDate { get; set; }
        public string? FirmwareVersion { get; set; }
        public decimal? InitialBatteryLevel { get; set; }
        public int? InitialSignalStrength { get; set; }
        public decimal? InitialSnr { get; set; }
        public int TransmissionIntervalMinutes { get; set; }
        public bool SupportsValveControl { get; set; }
        public bool SupportsTwoWayCommunication { get; set; }
        public string? Notes { get; set; }
    }
}
