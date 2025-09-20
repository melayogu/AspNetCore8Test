namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class CreateLoRaAlertDto
    {
        public int DeviceId { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
