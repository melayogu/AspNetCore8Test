using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 管線監控模型
    /// </summary>
    public class Pipeline
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PipelineNumber { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // Main, Distribution, Service
        
        public decimal Length { get; set; } = 0; // 長度（公里）
        
        public decimal Diameter { get; set; } = 0; // 直徑（毫米）
        
        public decimal MaxPressure { get; set; } = 0; // 最大壓力（kPa）
        
        public decimal OperatingPressure { get; set; } = 0; // 操作壓力（kPa）
        
        [StringLength(100)]
        public string Material { get; set; } = string.Empty; // 材質
        
        public DateTime InstallationDate { get; set; }
        
        public DateTime? LastInspectionDate { get; set; }
        
        public DateTime? NextInspectionDate { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Maintenance, Decommissioned
        
        [StringLength(500)]
        public string Location { get; set; } = string.Empty;
        
        public decimal Latitude { get; set; } = 0;
        
        public decimal Longitude { get; set; } = 0;
        
        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;
        
        // 導航屬性
        public virtual ICollection<PipelineMonitoring> MonitoringData { get; set; } = new List<PipelineMonitoring>();
        public virtual ICollection<PipelineAlert> Alerts { get; set; } = new List<PipelineAlert>();
    }
}