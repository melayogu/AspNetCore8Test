using System;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 無線抄表系統的儀表板概覽資料
    /// </summary>
    public class LoRaOverviewDto
    {
        public int TotalDevices { get; set; }
        public int ActiveDevices { get; set; }
        public int AlertDevices { get; set; }
        public double AverageSignalStrength { get; set; }
        public double PacketSuccessRate { get; set; }
        public int DailyReadings { get; set; }
        public int MonthlyReadings { get; set; }
        public double CoveragePercentage { get; set; }
        public int GatewayCount { get; set; }
        public int OfflineGatewayCount { get; set; }
        public int BatteryCriticalCount { get; set; }
        public int FirmwareOutdatedCount { get; set; }
        public int RecentAlertCount { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }
}
