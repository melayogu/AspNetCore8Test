namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class LoRaGatewayDto
    {
        public int Id { get; set; }
        public string GatewayCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime LastHeartbeat { get; set; }
        public DateTime InstalledDate { get; set; }
        public string FirmwareVersion { get; set; } = string.Empty;
        public string ChannelPlan { get; set; } = string.Empty;
        public string UplinkFrequency { get; set; } = string.Empty;
        public string DownlinkFrequency { get; set; } = string.Empty;
        public decimal CoverageRadiusKm { get; set; }
        public string BackhaulType { get; set; } = string.Empty;
        public double PacketForwardSuccessRate { get; set; }
        public int ConnectedDevices { get; set; }
    }
}
