namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 代表 LoRa 微電腦抄表系統中的終端設備
    /// </summary>
    public class LoRaDevice
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MeterType { get; set; } = string.Empty;
        public string FirmwareVersion { get; set; } = string.Empty;
        public double BatteryLevel { get; set; }
        public string InstallLocation { get; set; } = string.Empty;
        public DateTime InstallDate { get; set; }
        public DateTime LastCommunication { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
        public string GatewayEui { get; set; } = string.Empty;
        public string ApplicationKey { get; set; } = string.Empty;
        public string FrequencyPlan { get; set; } = string.Empty;
        public double AlertThreshold { get; set; }
        public bool SupportsClassC { get; set; }
        public double LastSignalStrength { get; set; }
        public decimal LastReadingValue { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
