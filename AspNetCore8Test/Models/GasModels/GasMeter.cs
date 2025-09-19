using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 瓦斯錶資料模型
    /// </summary>
    public class GasMeter
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string MeterNumber { get; set; } = string.Empty;
        
        public int CustomerId { get; set; }
        
        [StringLength(500)]
        public string InstallationAddress { get; set; } = string.Empty;
        
        public DateTime InstallationDate { get; set; }
        
        public decimal LastReading { get; set; } = 0;
        
        public DateTime LastReadingDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string MeterType { get; set; } = string.Empty; // 智慧錶、傳統錶
        
        [StringLength(50)]
        public string MeterStatus { get; set; } = "Active"; // Active, Maintenance, Replaced
        
        public decimal Capacity { get; set; } = 0; // 錶容量
        
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;
        
        public DateTime? LastMaintenanceDate { get; set; }
        
        public DateTime? NextMaintenanceDate { get; set; }
        
        // 導航屬性
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
    }
}