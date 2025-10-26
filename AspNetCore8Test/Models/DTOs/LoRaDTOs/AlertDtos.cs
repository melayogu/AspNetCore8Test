using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateLoRaAlertDto
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string DeviceEui { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 16)]
        public string GatewayEui { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AlertType { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Severity { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(500)]
        public string SuggestedAction { get; set; } = string.Empty;

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }

    public class AcknowledgeLoRaAlertDto
    {
        public bool IsAcknowledged { get; set; } = true;

        public DateTime? AcknowledgedAt { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? SuggestedAction { get; set; }
    }

    public class LoRaAlertDto
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
