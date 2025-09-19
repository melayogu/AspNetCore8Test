using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 文件類型列舉
    /// </summary>
    public enum DocumentType
    {
        [Display(Name = "委託書")]
        PowerOfAttorney = 1,
        [Display(Name = "身分證明")]
        IdentityDocument = 2,
        [Display(Name = "所有權狀")]
        OwnershipCertificate = 3,
        [Display(Name = "買賣契約書")]
        SalesContract = 4,
        [Display(Name = "抵押權設定契約書")]
        MortgageContract = 5,
        [Display(Name = "地籍圖謄本")]
        CadastralMap = 6,
        [Display(Name = "土地登記簿謄本")]
        LandRegistrationTranscript = 7,
        [Display(Name = "建物登記簿謄本")]
        BuildingRegistrationTranscript = 8,
        [Display(Name = "測量成果圖")]
        SurveyResultMap = 9,
        [Display(Name = "其他")]
        Other = 10
    }

    /// <summary>
    /// 案件文件模型
    /// </summary>
    public class CaseDocument
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "案件為必填項目")]
        [Display(Name = "相關案件")]
        public int CaseId { get; set; }

        [Required(ErrorMessage = "文件名稱為必填項目")]
        [StringLength(100, ErrorMessage = "文件名稱長度不能超過100個字元")]
        [Display(Name = "文件名稱")]
        public string DocumentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "文件類型為必填項目")]
        [Display(Name = "文件類型")]
        public DocumentType DocumentType { get; set; }

        [Display(Name = "檔案路徑")]
        [StringLength(500, ErrorMessage = "檔案路徑長度不能超過500個字元")]
        public string? FilePath { get; set; }

        [Display(Name = "檔案大小(Bytes)")]
        public long? FileSize { get; set; }

        [Display(Name = "檔案類型")]
        [StringLength(10, ErrorMessage = "檔案類型長度不能超過10個字元")]
        public string? FileExtension { get; set; }

        [Display(Name = "上傳日期")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Display(Name = "版本號")]
        public int Version { get; set; } = 1;

        [Display(Name = "是否為必要文件")]
        public bool IsRequired { get; set; } = false;

        [Display(Name = "是否已收到")]
        public bool IsReceived { get; set; } = false;

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        // 導航屬性
        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; } = null!;
    }
}