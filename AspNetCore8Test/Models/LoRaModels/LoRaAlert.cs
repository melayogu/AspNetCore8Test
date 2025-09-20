namespace AspNetCore8Test.Models.LoRaModels
{
    /// <summary>
    /// 表示 LoRa 無線抄表系統所觸發的警報。
    /// </summary>
    public class LoRaAlert
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public string DeviceEui { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public string? AcknowledgedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolvedBy { get; set; }
        public bool IsResolved { get; set; }
    }
}
