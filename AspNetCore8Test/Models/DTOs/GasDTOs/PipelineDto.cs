using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    public class PipelineMonitoringDto
    {
        public int Id { get; set; }
        public int PipelineId { get; set; }
        public string PipelineNumber { get; set; } = string.Empty;
        public string PipelineName { get; set; } = string.Empty;
        public DateTime RecordTime { get; set; }
        public decimal Pressure { get; set; }
        public decimal FlowRate { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public bool IsAlarmTriggered { get; set; }
        public string AlarmType { get; set; } = string.Empty;
        public string AlarmMessage { get; set; } = string.Empty;
        public string DataSource { get; set; } = string.Empty;
        public bool IsValidData { get; set; }
    }

    public class PipelineAlertDto
    {
        public int Id { get; set; }
        public int PipelineId { get; set; }
        public string PipelineNumber { get; set; } = string.Empty;
        public string PipelineName { get; set; } = string.Empty;
        public DateTime AlertTime { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? AcknowledgedTime { get; set; }
        public string AcknowledgedBy { get; set; } = string.Empty;
        public DateTime? ResolvedTime { get; set; }
        public string ResolvedBy { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
    }

    public class CreatePipelineAlertDto
    {
        [Required(ErrorMessage = "管線ID是必填的")]
        public int PipelineId { get; set; }
        
        [Required(ErrorMessage = "警報類型是必填的")]
        [StringLength(50)]
        public string AlertType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "嚴重程度是必填的")]
        [StringLength(50)]
        public string Severity { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "標題是必填的")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "描述是必填的")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }

    public class PipelineDto
    {
        public int Id { get; set; }
        public string PipelineNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Length { get; set; }
        public decimal Diameter { get; set; }
        public decimal MaxPressure { get; set; }
        public decimal OperatingPressure { get; set; }
        public string Material { get; set; } = string.Empty;
        public DateTime InstallationDate { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}