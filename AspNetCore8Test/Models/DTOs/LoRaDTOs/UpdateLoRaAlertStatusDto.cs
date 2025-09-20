namespace AspNetCore8Test.Models.DTOs.LoRaDTOs
{
    public class UpdateLoRaAlertStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? ResolvedBy { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}
