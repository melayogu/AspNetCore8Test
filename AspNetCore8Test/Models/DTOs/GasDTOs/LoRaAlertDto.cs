using System;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    /// <summary>
    /// LoRa 系統警報資訊
    /// </summary>
    public class LoRaAlertDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string DeviceNumber { get; set; } = string.Empty;
        public string MeterNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RecommendedAction { get; set; } = string.Empty;
        public DateTime? AcknowledgedAt { get; set; }
        public string? AcknowledgedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolvedBy { get; set; }
    }
}
