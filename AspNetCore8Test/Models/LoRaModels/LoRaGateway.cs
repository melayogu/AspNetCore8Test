namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 表示負責 LoRa 裝置傳輸的閘道器。
    /// </summary>
    public class LoRaGateway
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
