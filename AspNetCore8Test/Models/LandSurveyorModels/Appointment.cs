using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 預約狀態列舉
    /// </summary>
    public enum AppointmentStatus
    {
        [Display(Name = "已預約")]
        Scheduled = 1,
        [Display(Name = "已確認")]
        Confirmed = 2,
        [Display(Name = "已完成")]
        Completed = 3,
        [Display(Name = "已取消")]
        Cancelled = 4,
        [Display(Name = "延期")]
        Postponed = 5
    }

    /// <summary>
    /// 服務類型列舉
    /// </summary>
    public enum ServiceType
    {
        [Display(Name = "諮詢服務")]
        Consultation = 1,
        [Display(Name = "文件準備")]
        DocumentPreparation = 2,
        [Display(Name = "現場會勘")]
        SiteInspection = 3,
        [Display(Name = "案件說明")]
        CaseExplanation = 4,
        [Display(Name = "簽約")]
        ContractSigning = 5,
        [Display(Name = "其他")]
        Other = 6
    }

    /// <summary>
    /// 預約服務模型
    /// </summary>
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "客戶為必填項目")]
        [Display(Name = "客戶")]
        public int CustomerId { get; set; }

        [Display(Name = "相關案件")]
        public int? CaseId { get; set; }

        [Required(ErrorMessage = "預約標題為必填項目")]
        [StringLength(100, ErrorMessage = "預約標題長度不能超過100個字元")]
        [Display(Name = "預約標題")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "服務類型為必填項目")]
        [Display(Name = "服務類型")]
        public ServiceType ServiceType { get; set; }

        [Required(ErrorMessage = "預約日期為必填項目")]
        [Display(Name = "預約日期")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "預約時間為必填項目")]
        [Display(Name = "預約時間")]
        public TimeSpan AppointmentTime { get; set; }

        [Display(Name = "預計時長(分鐘)")]
        public int EstimatedDuration { get; set; } = 60;

        [Display(Name = "預約狀態")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        [Display(Name = "地點")]
        [StringLength(200, ErrorMessage = "地點長度不能超過200個字元")]
        public string? Location { get; set; }

        [Display(Name = "預約說明")]
        [StringLength(500, ErrorMessage = "預約說明長度不能超過500個字元")]
        public string? Description { get; set; }

        [Display(Name = "客戶聯絡電話")]
        [StringLength(20, ErrorMessage = "客戶聯絡電話長度不能超過20個字元")]
        public string? ContactPhone { get; set; }

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

        [ForeignKey("CaseId")]
        public virtual Case? Case { get; set; }
    }
}