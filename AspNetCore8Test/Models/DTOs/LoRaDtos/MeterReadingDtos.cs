namespace AspNetCore8Test.Models.DTOs.LoRaDtos
{
    public class MeterReadingDto
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

    public class CreateMeterReadingDto
    {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int SignalStrength { get; set; }
        public double SignalToNoiseRatio { get; set; }
        public double BatteryLevel { get; set; }
        public string TransmissionStatus { get; set; } = string.Empty;
    }
}
