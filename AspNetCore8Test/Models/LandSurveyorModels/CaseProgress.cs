using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 案件進度追蹤模型
    /// </summary>
    public class CaseProgress
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "案件為必填項目")]
        [Display(Name = "相關案件")]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "進度標題為必填項目")]
        [StringLength(100, ErrorMessage = "進度標題長度不能超過100個字元")]
        [Display(Name = "進度標題")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "進度描述")]
        [StringLength(1000, ErrorMessage = "進度描述長度不能超過1000個字元")]
        public string? Description { get; set; }

        [Display(Name = "進度狀態")]
        public CaseStatus Status { get; set; }

        [Display(Name = "完成日期")]
        public DateTime? CompletedDate { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "負責人員")]
        [StringLength(50, ErrorMessage = "負責人員長度不能超過50個字元")]
        public string? AssignedTo { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        // 導航屬性
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; } = null!;
    }
}