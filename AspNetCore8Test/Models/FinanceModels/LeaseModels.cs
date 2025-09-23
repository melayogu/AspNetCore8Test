using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.FinanceModels
{
    // 信用狀申請模型
    public class LetterOfCreditApplication
    {
        public int Id { get; set; }
        
        [Display(Name = "申請人")]
        public string ApplicantName { get; set; } = string.Empty;
        
        [Display(Name = "受益人")]
        public string BeneficiaryName { get; set; } = string.Empty;
        
        [Display(Name = "信用狀金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LcAmount { get; set; }
        
        [Display(Name = "幣別")]
        public string Currency { get; set; } = string.Empty;
        
        [Display(Name = "匯率")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ExchangeRate { get; set; }
        
        [Display(Name = "機具說明")]
        public string EquipmentDescription { get; set; } = string.Empty;
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "到期日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ExpiryDate { get; set; }
        
        [Display(Name = "信用狀號碼")]
        public string LcNumber { get; set; } = string.Empty;
    }

    // 重機租賃模型
    public class MotorcycleLease
    {
        public int Id { get; set; }
        
        [Display(Name = "客戶姓名")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "品牌")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "型號")]
        public string Model { get; set; } = string.Empty;
        
        [Display(Name = "排氣量(cc)")]
        public int EngineCapacity { get; set; }
        
        [Display(Name = "租賃類型")]
        public string LeaseType { get; set; } = string.Empty;
        
        [Display(Name = "總金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "租期(月)")]
        public int LeasePeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "租期開始")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }
        
        [Display(Name = "租期結束")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "里程數")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Mileage { get; set; }
    }

    // 租賃合約模型
    public class LeaseContract
    {
        public int Id { get; set; }
        
        [Display(Name = "合約編號")]
        public string ContractNumber { get; set; } = string.Empty;
        
        [Display(Name = "承租人")]
        public string LesseeName { get; set; } = string.Empty;
        
        [Display(Name = "資產類型")]
        public string AssetType { get; set; } = string.Empty;
        
        [Display(Name = "資產描述")]
        public string AssetDescription { get; set; } = string.Empty;
        
        [Display(Name = "租賃金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LeaseAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "租期(月)")]
        public int LeasePeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "簽約日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SignDate { get; set; }
        
        [Display(Name = "租期開始")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }
        
        [Display(Name = "租期結束")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }
    }

    // 租賃資產模型
    public class LeaseAsset
    {
        public int Id { get; set; }
        
        [Display(Name = "資產編號")]
        public string AssetNumber { get; set; } = string.Empty;
        
        [Display(Name = "資產類型")]
        public string AssetType { get; set; } = string.Empty;
        
        [Display(Name = "品牌")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "型號")]
        public string Model { get; set; } = string.Empty;
        
        [Display(Name = "購置日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PurchaseDate { get; set; }
        
        [Display(Name = "購置價格")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PurchasePrice { get; set; }
        
        [Display(Name = "目前價值")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal CurrentValue { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "位置")]
        public string Location { get; set; } = string.Empty;
        
        [Display(Name = "狀況")]
        public string Condition { get; set; } = string.Empty;
        
        [Display(Name = "折舊率(%)")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal DepreciationRate { get; set; }
    }
}