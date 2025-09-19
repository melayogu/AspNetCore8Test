using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 管線監控數據模型
    /// </summary>
    public class PipelineMonitoring
    {
        public int Id { get; set; }
        
        public int PipelineId { get; set; }
        
        public DateTime RecordTime { get; set; } = DateTime.Now;
        
        public decimal Pressure { get; set; } = 0; // 壓力（kPa）
        
        public decimal FlowRate { get; set; } = 0; // 流量（立方米/小時）
        
        public decimal Temperature { get; set; } = 0; // 溫度（攝氏度）
        
        public decimal Humidity { get; set; } = 0; // 濕度（%）
        
        public bool IsAlarmTriggered { get; set; } = false;
        
        [StringLength(50)]
        public string AlarmType { get; set; } = string.Empty; // Pressure, Flow, Temperature, Leak
        
        [StringLength(500)]
        public string AlarmMessage { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string DataSource { get; set; } = string.Empty; // Sensor, Manual, Calculated
        
        public bool IsValidData { get; set; } = true;
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        // 導航屬性
        public virtual Pipeline Pipeline { get; set; } = null!;
    }
}