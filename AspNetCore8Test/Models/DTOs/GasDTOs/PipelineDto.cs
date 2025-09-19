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
        public string? Description { get; set; }
        public decimal Pressure { get; set; }
    }

    public class CreatePipelineDto
    {
        [Required(ErrorMessage = "管線編號是必填的")]
        [StringLength(50)]
        public string PipelineNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "管線名稱是必填的")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "管線類型是必填的")]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "長度是必填的")]
        [Range(0.1, 10000, ErrorMessage = "長度必須在0.1到10000公里之間")]
        public decimal Length { get; set; }

        [Required(ErrorMessage = "直徑是必填的")]
        [Range(0.1, 5, ErrorMessage = "直徑必須在0.1到5公尺之間")]
        public decimal Diameter { get; set; }

        [Required(ErrorMessage = "最大壓力是必填的")]
        [Range(1, 200, ErrorMessage = "最大壓力必須在1到200bar之間")]
        public decimal MaxPressure { get; set; }

        [Required(ErrorMessage = "操作壓力是必填的")]
        [Range(0.1, 150, ErrorMessage = "操作壓力必須在0.1到150bar之間")]
        public decimal OperatingPressure { get; set; }

        [Required(ErrorMessage = "材質是必填的")]
        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        [Required(ErrorMessage = "安裝日期是必填的")]
        public DateTime InstallationDate { get; set; }

        [Required(ErrorMessage = "位置是必填的")]
        [StringLength(500)]
        public string Location { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
        public decimal Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
        public decimal Longitude { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime? LastInspectionDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
    }

    public class UpdatePipelineDto
    {
        [Required(ErrorMessage = "管線名稱是必填的")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "管線類型是必填的")]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "長度是必填的")]
        [Range(0.1, 10000, ErrorMessage = "長度必須在0.1到10000公里之間")]
        public decimal Length { get; set; }

        [Required(ErrorMessage = "直徑是必填的")]
        [Range(0.1, 5, ErrorMessage = "直徑必須在0.1到5公尺之間")]
        public decimal Diameter { get; set; }

        [Required(ErrorMessage = "最大壓力是必填的")]
        [Range(1, 200, ErrorMessage = "最大壓力必須在1到200bar之間")]
        public decimal MaxPressure { get; set; }

        [Required(ErrorMessage = "操作壓力是必填的")]
        [Range(0.1, 150, ErrorMessage = "操作壓力必須在0.1到150bar之間")]
        public decimal OperatingPressure { get; set; }

        [Required(ErrorMessage = "材質是必填的")]
        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        [Required(ErrorMessage = "狀態是必填的")]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "位置是必填的")]
        [StringLength(500)]
        public string Location { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "緯度必須在-90到90之間")]
        public decimal Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "經度必須在-180到180之間")]
        public decimal Longitude { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime? LastInspectionDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
    }
}