namespace AspNetCore8Test.Models.LoRaModels
{
    public class LoRaAlert
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = "Medium";
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active";
        public DateTime? AcknowledgedAt { get; set; }
        public string? AcknowledgedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolvedBy { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}
