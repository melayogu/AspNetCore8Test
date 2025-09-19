using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 帳單資料模型
    /// </summary>
    public class Bill
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string BillNumber { get; set; } = string.Empty;
        
        public int CustomerId { get; set; }
        
        public int GasMeterId { get; set; }
        
        public DateTime BillDate { get; set; } = DateTime.Now;
        
        public DateTime DueDate { get; set; }
        
        public DateTime BillingPeriodStart { get; set; }
        
        public DateTime BillingPeriodEnd { get; set; }
        
        public decimal PreviousReading { get; set; } = 0;
        
        public decimal CurrentReading { get; set; } = 0;
        
        public decimal Usage { get; set; } = 0; // 使用量（立方米）
        
        public decimal UnitPrice { get; set; } = 0; // 單價
        
        public decimal BasicCharge { get; set; } = 0; // 基本費
        
        public decimal UsageCharge { get; set; } = 0; // 使用費
        
        public decimal TaxAmount { get; set; } = 0; // 稅額
        
        public decimal TotalAmount { get; set; } = 0; // 總金額
        
        public decimal PaidAmount { get; set; } = 0; // 已付金額
        
        public decimal BalanceAmount { get; set; } = 0; // 餘額
        
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue, Cancelled
        
        public DateTime? PaymentDate { get; set; }
        
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Transfer, CreditCard, Check
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
        
        // 導航屬性
        public virtual Customer Customer { get; set; } = null!;
        public virtual GasMeter GasMeter { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}