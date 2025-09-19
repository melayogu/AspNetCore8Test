using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.GasModels
{
    /// <summary>
    /// 客戶資料模型
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CustomerNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // 住宅、商業、工業
        
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string TaxId { get; set; } = string.Empty; // 統一編號
        
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        public string Status { get; set; } = "Active"; // Active, Suspended, Terminated
        
        // 導航屬性
        public virtual ICollection<GasMeter> GasMeters { get; set; } = new List<GasMeter>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}