namespace AspNetCore8Test.Models.DTOs.LoRaDtos
{
    public class LoRaNetworkSummaryDto
    {
        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int OfflineDevices { get; set; }
        public int AlertingDevices { get; set; }
        public double AverageBatteryLevel { get; set; }
        public double AverageSignalStrength { get; set; }
        public int GatewaysOnline { get; set; }
        public int GatewaysOffline { get; set; }
        public int ReadingsLast24Hours { get; set; }
        public double SuccessfulTransmissionRate { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
