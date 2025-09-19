using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 財務管理模型
    /// </summary>
    public class FinancialRecord
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "案件為必填項目")]
        [Display(Name = "相關案件")]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "交易類型為必填項目")]
        [StringLength(20, ErrorMessage = "交易類型長度不能超過20個字元")]
        [Display(Name = "交易類型")]
        public string TransactionType { get; set; } = string.Empty; // 收入、支出

        [Required(ErrorMessage = "金額為必填項目")]
        [Display(Name = "金額")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "交易日期")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Display(Name = "付款方式")]
        [StringLength(20, ErrorMessage = "付款方式長度不能超過20個字元")]
        public string? PaymentMethod { get; set; } // 現金、轉帳、支票等

        [Display(Name = "發票號碼")]
        [StringLength(20, ErrorMessage = "發票號碼長度不能超過20個字元")]
        public string? InvoiceNumber { get; set; }

        [Display(Name = "收據號碼")]
        [StringLength(20, ErrorMessage = "收據號碼長度不能超過20個字元")]
        public string? ReceiptNumber { get; set; }

        [Display(Name = "說明")]
        [StringLength(200, ErrorMessage = "說明長度不能超過200個字元")]
        public string? Description { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        // 導航屬性
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; } = null!;
    }
}