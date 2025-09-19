using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 付款記錄模型
    /// </summary>
    public class Payment
    {
        public int Id { get; set; }
        
        public int BillId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PaymentNumber { get; set; } = string.Empty;
        
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        
        public decimal Amount { get; set; } = 0;
        
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Transfer, CreditCard, Check
        
        [StringLength(100)]
        public string ReferenceNumber { get; set; } = string.Empty; // 交易參考號碼
        
        [StringLength(50)]
        public string Status { get; set; } = "Completed"; // Completed, Pending, Failed, Cancelled
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string ProcessedBy { get; set; } = string.Empty; // 處理人員
        
        // 導航屬性
        public virtual Bill Bill { get; set; } = null!;
    }
}