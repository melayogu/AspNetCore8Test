using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.DTOs.GasDTOs
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "客戶編號是必填的")]
        [StringLength(100, ErrorMessage = "客戶編號最多100個字元")]
        public string CustomerNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "客戶姓名是必填的")]
        [StringLength(200, ErrorMessage = "客戶姓名最多200個字元")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "客戶類型是必填的")]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // 住宅、商業、工業
        
        [Required(ErrorMessage = "地址是必填的")]
        [StringLength(500, ErrorMessage = "地址最多500個字元")]
        public string Address { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "請輸入有效的電話號碼")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "統一編號最多20個字元")]
        public string TaxId { get; set; } = string.Empty;
    }

    public class UpdateCustomerDto : CreateCustomerDto
    {
        public int Id { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(50)]
        public string Status { get; set; } = "Active";
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}