using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 不動產登記模型
    /// </summary>
    public class RealEstateRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "登記案件為必填項目")]
        [Display(Name = "相關案件")]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "地段為必填項目")]
        [StringLength(50, ErrorMessage = "地段長度不能超過50個字元")]
        [Display(Name = "地段")]
        public string LandSection { get; set; } = string.Empty;

        [Required(ErrorMessage = "地號為必填項目")]
        [StringLength(20, ErrorMessage = "地號長度不能超過20個字元")]
        [Display(Name = "地號")]
        public string LandNumber { get; set; } = string.Empty;

        [Display(Name = "建號")]
        [StringLength(20, ErrorMessage = "建號長度不能超過20個字元")]
        public string? BuildingNumber { get; set; }

        [Display(Name = "面積(平方公尺)")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Area { get; set; }

        [Display(Name = "使用分區")]
        [StringLength(50, ErrorMessage = "使用分區長度不能超過50個字元")]
        public string? ZoningDistrict { get; set; }

        [Display(Name = "地目")]
        [StringLength(20, ErrorMessage = "地目長度不能超過20個字元")]
        public string? LandUse { get; set; }

        [Display(Name = "等則")]
        [StringLength(10, ErrorMessage = "等則長度不能超過10個字元")]
        public string? Grade { get; set; }

        [Display(Name = "權利範圍")]
        [StringLength(20, ErrorMessage = "權利範圍長度不能超過20個字元")]
        public string? RightsPortion { get; set; }

        [Display(Name = "登記原因")]
        [StringLength(100, ErrorMessage = "登記原因長度不能超過100個字元")]
        public string? RegistrationReason { get; set; }

        [Display(Name = "登記日期")]
        public DateTime? RegistrationDate { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新日期")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        // 導航屬性
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; } = null!;
    }
}