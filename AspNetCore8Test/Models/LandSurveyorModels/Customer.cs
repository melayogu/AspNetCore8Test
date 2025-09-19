using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore8Test.Models.LandSurveyorModels
{
    /// <summary>
    /// 客戶資料模型
    /// </summary>
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "客戶姓名為必填項目")]
        [StringLength(50, ErrorMessage = "客戶姓名長度不能超過50個字元")]
        [Display(Name = "客戶姓名")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "身分證字號為必填項目")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "身分證字號必須為10碼")]
        [Display(Name = "身分證字號")]
        public string IdNumber { get; set; } = string.Empty;

        [Phone(ErrorMessage = "請輸入有效的電話號碼")]
        [Display(Name = "電話")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        [Display(Name = "電子郵件")]
        public string? Email { get; set; }

        [Display(Name = "地址")]
        [StringLength(200, ErrorMessage = "地址長度不能超過200個字元")]
        public string? Address { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新日期")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "備註長度不能超過500個字元")]
        public string? Notes { get; set; }

        [Display(Name = "是否為活躍客戶")]
        public bool IsActive { get; set; } = true;

        // 導航屬性
        public virtual ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}