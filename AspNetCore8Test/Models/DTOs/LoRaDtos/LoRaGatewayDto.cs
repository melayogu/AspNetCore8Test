namespace AspNetCore8Test.Models.DTOs.LoRaDtos
{
    public class LoRaGatewayDto
    {
        public int Id { get; set; }
        public string GatewayEui { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FirmwareVersion { get; set; } = string.Empty;
        public string BackhaulType { get; set; } = string.Empty;
        public string FrequencyPlan { get; set; } = string.Empty;
        public DateTime LastHeartbeatAt { get; set; }
        public bool IsOnline { get; set; }
        public int ConnectedDevices { get; set; }
    }
}
