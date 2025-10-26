namespace AspNetCore8Test.Models.DTOs.LoRaDtos
{
    public class LoRaAlertDto
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

    public class AlertActionDto
    {
        public string OperatorName { get; set; } = string.Empty;
    }
}
