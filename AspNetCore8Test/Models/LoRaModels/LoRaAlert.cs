namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 代表 LoRa 抄表系統中的即時告警
    /// </summary>
    public class LoRaAlert
    {
        public int Id { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public string GatewayEui { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string SuggestedAction { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
