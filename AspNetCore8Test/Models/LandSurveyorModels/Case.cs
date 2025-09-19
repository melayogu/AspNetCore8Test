using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 案件狀態列舉
    /// </summary>
    public enum CaseStatus
    {
        [Display(Name = "已收件")]
        Received = 1,
        [Display(Name = "處理中")]
        Processing = 2,
        [Display(Name = "待補件")]
        PendingDocuments = 3,
        [Display(Name = "已送件")]
        Submitted = 4,
        [Display(Name = "已完成")]
        Completed = 5,
        [Display(Name = "已取消")]
        Cancelled = 6
    }

    /// <summary>
    /// 案件類型列舉
    /// </summary>
    public enum CaseType
    {
        [Display(Name = "所有權移轉")]
        OwnershipTransfer = 1,
        [Display(Name = "抵押權設定")]
        MortgageRegistration = 2,
        [Display(Name = "抵押權塗銷")]
        MortgageCancellation = 3,
        [Display(Name = "建物保存登記")]
        BuildingPreservation = 4,
        [Display(Name = "土地分割")]
        LandSubdivision = 5,
        [Display(Name = "土地合併")]
        LandConsolidation = 6,
        [Display(Name = "地目變更")]
        LandUseChange = 7,
        [Display(Name = "測量")]
        Survey = 8,
        [Display(Name = "其他")]
        Other = 9
    }

    /// <summary>
    /// 案件管理模型
    /// </summary>
    public class Case
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "案件編號為必填項目")]
        [StringLength(20, ErrorMessage = "案件編號長度不能超過20個字元")]
        [Display(Name = "案件編號")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "案件名稱為必填項目")]
        [StringLength(100, ErrorMessage = "案件名稱長度不能超過100個字元")]
        [Display(Name = "案件名稱")]
        public string CaseName { get; set; } = string.Empty;

        [Required(ErrorMessage = "案件類型為必填項目")]
        [Display(Name = "案件類型")]
        public CaseType CaseType { get; set; }

        [Display(Name = "案件狀態")]
        public CaseStatus Status { get; set; } = CaseStatus.Received;

        [Required(ErrorMessage = "客戶為必填項目")]
        [Display(Name = "客戶")]
        public int CustomerId { get; set; }

        [Display(Name = "收件日期")]
        public DateTime ReceivedDate { get; set; } = DateTime.Now;

        [Display(Name = "預計完成日期")]
        public DateTime? ExpectedCompletionDate { get; set; }

        [Display(Name = "實際完成日期")]
        public DateTime? ActualCompletionDate { get; set; }

        [Display(Name = "案件描述")]
        [StringLength(1000, ErrorMessage = "案件描述長度不能超過1000個字元")]
        public string? Description { get; set; }

        [Display(Name = "費用")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Fee { get; set; }

        [Display(Name = "已收款")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaidAmount { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新日期")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        // 導航屬性
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;
        
        public virtual ICollection<CaseDocument> CaseDocuments { get; set; } = new List<CaseDocument>();
        public virtual ICollection<CaseProgress> CaseProgresses { get; set; } = new List<CaseProgress>();
    }
}