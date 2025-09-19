using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    public class CreateBillDto
    {
        [Required(ErrorMessage = "帳單編號是必填的")]
        [StringLength(50)]
        public string BillNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "客戶ID是必填的")]
        public int CustomerId { get; set; }
        
        [Required(ErrorMessage = "瓦斯錶ID是必填的")]
        public int GasMeterId { get; set; }
        
        [Required(ErrorMessage = "帳單日期是必填的")]
        public DateTime BillDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "到期日是必填的")]
        public DateTime DueDate { get; set; }
        
        [Required(ErrorMessage = "計費期間開始日是必填的")]
        public DateTime BillingPeriodStart { get; set; }
        
        [Required(ErrorMessage = "計費期間結束日是必填的")]
        public DateTime BillingPeriodEnd { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "前期讀數必須大於等於0")]
        public decimal PreviousReading { get; set; } = 0;
        
        [Range(0, double.MaxValue, ErrorMessage = "本期讀數必須大於等於0")]
        public decimal CurrentReading { get; set; } = 0;
        
        [Range(0, double.MaxValue, ErrorMessage = "單價必須大於等於0")]
        public decimal UnitPrice { get; set; } = 0;
        
        [Range(0, double.MaxValue, ErrorMessage = "基本費必須大於等於0")]
        public decimal BasicCharge { get; set; } = 0;
    }

    public class BillDto
    {
        public int Id { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int GasMeterId { get; set; }
        public string MeterNumber { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime BillingPeriodStart { get; set; }
        public DateTime BillingPeriodEnd { get; set; }
        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }
        public decimal Usage { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal BasicCharge { get; set; }
        public decimal UsageCharge { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class PaymentDto
    {
        [Required(ErrorMessage = "帳單ID是必填的")]
        public int BillId { get; set; }
        
        [Required(ErrorMessage = "付款金額是必填的")]
        [Range(0.01, double.MaxValue, ErrorMessage = "付款金額必須大於0")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "付款方式是必填的")]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string ReferenceNumber { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }
}