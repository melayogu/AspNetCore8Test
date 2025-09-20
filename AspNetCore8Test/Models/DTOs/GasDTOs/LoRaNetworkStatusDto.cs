namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 網路健康度指標
    /// </summary>
    public class LoRaNetworkStatusDto
    {
        public string Region { get; set; } = string.Empty;
        public int DeviceCount { get; set; }
        public double AverageSignalStrength { get; set; }
        public double PacketSuccessRate { get; set; }
        public double CoverageRate { get; set; }
        public int ActiveGateways { get; set; }
        public int AlertCount { get; set; }
    }
}
