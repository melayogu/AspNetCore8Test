namespace AspNetCore8Test.Models.DTOs.LoRaDtos
{
    public class LoRaDeviceDto
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public int GatewayId { get; set; }
        public string InstallationLocation { get; set; } = string.Empty;
        public string InstallationNotes { get; set; } = string.Empty;
        public DateTime InstalledAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastCommunicationAt { get; set; }
        public double LastReadingValue { get; set; }
        public string LastReadingUnit { get; set; } = string.Empty;
        public double BatteryLevel { get; set; }
        public int SignalStrength { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public bool TamperDetected { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateLoRaDeviceDto
    {
        public string DeviceEui { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public int GatewayId { get; set; }
        public string InstallationLocation { get; set; } = string.Empty;
        public string InstallationNotes { get; set; } = string.Empty;
        public DateTime InstalledAt { get; set; }
    }

    public class UpdateLoRaDeviceDto : CreateLoRaDeviceDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
        public double BatteryLevel { get; set; }
        public int SignalStrength { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public bool TamperDetected { get; set; }
        public double LastReadingValue { get; set; }
        public string LastReadingUnit { get; set; } = string.Empty;
        public DateTime LastCommunicationAt { get; set; }
    }
}
