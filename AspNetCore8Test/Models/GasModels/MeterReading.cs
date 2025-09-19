using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 抄錶記錄模型
    /// </summary>
    public class MeterReading
    {
        public int Id { get; set; }
        
        public int GasMeterId { get; set; }
        
        public decimal Reading { get; set; } = 0;
        
        public DateTime ReadingDate { get; set; } = DateTime.Now;
        
        public decimal Usage { get; set; } = 0; // 使用量
        
        [StringLength(50)]
        public string ReadingType { get; set; } = string.Empty; // Manual, Automatic, Estimated
        
        [StringLength(100)]
        public string ReaderName { get; set; } = string.Empty; // 抄錶員姓名
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        public bool IsVerified { get; set; } = false;
        
        public DateTime? VerifiedDate { get; set; }
        
        [StringLength(100)]
        public string VerifiedBy { get; set; } = string.Empty;
        
        // 導航屬性
        public virtual GasMeter GasMeter { get; set; } = null!;
    }
}