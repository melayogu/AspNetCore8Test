namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class LoRaDashboardStatsDto
    {
        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int MaintenanceDevices { get; set; }
        public int OfflineDevices { get; set; }
        public int TotalGateways { get; set; }
        public int GatewayOnline { get; set; }
        public int GatewayOffline { get; set; }
        public double AverageBatteryLevel { get; set; }
        public double AverageSignalStrength { get; set; }
        public int ActiveAlerts { get; set; }
        public int DailyReadings { get; set; }
        public int MonthlyReadings { get; set; }
    }
}
