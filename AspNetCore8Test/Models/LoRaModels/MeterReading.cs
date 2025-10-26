namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 表示裝置回傳的抄表資料。
    /// </summary>
    public class MeterReading
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int SignalStrength { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public double BatteryLevel { get; set; }
        public string TransmissionStatus { get; set; } = string.Empty;
        public int FrameCounter { get; set; }
    }
}
