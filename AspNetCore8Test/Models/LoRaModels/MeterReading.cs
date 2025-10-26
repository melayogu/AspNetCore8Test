namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 代表 LoRa 抄表設備上傳的讀值資料
    /// </summary>
    public class MeterReading
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public decimal ReadingValue { get; set; }
        public decimal Consumption { get; set; }
        public decimal FlowRate { get; set; }
        public double BatteryLevel { get; set; }
        public int Rssi { get; set; }
        public double Snr { get; set; }
        public bool IsEstimated { get; set; }
        public string Quality { get; set; } = string.Empty;
        public string PayloadHex { get; set; } = string.Empty;
    }
}
