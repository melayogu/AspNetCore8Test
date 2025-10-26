namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 代表 LoRa 微電腦抄表系統中的閘道器
    /// </summary>
    public class LoRaGateway
    {
        public int Id { get; set; }
        public string GatewayEui { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastCommunication { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ConnectedDevices { get; set; }
        public double PacketSuccessRate { get; set; }
        public string BackhaulType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
