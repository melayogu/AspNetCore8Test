using System;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 閘道器資訊
    /// </summary>
    public class LoRaGatewayDto
    {
        public int Id { get; set; }
        public string GatewayId { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime LastHeartbeat { get; set; }
        public int ConnectedDevices { get; set; }
        public double PacketSuccessRate { get; set; }
        public double AverageSignalStrength { get; set; }
        public double UptimePercentage { get; set; }
        public bool HasBackupPower { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
    }
}
