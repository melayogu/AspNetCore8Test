using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Test.Models.FinanceModels
{
    // 車輛分期模型
    public class VehicleFinance
    {
        public int Id { get; set; }
        
        [Display(Name = "客戶姓名")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "車輛類型")]
        public string VehicleType { get; set; } = string.Empty;
        
        [Display(Name = "品牌")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "型號")]
        public string Model { get; set; } = string.Empty;
        
        [Display(Name = "貸款金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LoanAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "貸款期數(月)")]
        public int LoanPeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "下次繳款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime NextPaymentDate { get; set; }
    }

    // 營建機具分期模型
    public class ConstructionEquipmentFinance
    {
        public int Id { get; set; }
        
        [Display(Name = "客戶名稱")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "機具類型")]
        public string EquipmentType { get; set; } = string.Empty;
        
        [Display(Name = "品牌")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "型號")]
        public string Model { get; set; } = string.Empty;
        
        [Display(Name = "貸款金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LoanAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "貸款期數(月)")]
        public int LoanPeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "下次繳款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime NextPaymentDate { get; set; }
    }

    // 產業機具分期模型
    public class IndustrialEquipmentFinance
    {
        public int Id { get; set; }
        
        [Display(Name = "客戶名稱")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "機具類型")]
        public string EquipmentType { get; set; } = string.Empty;
        
        [Display(Name = "品牌")]
        public string Brand { get; set; } = string.Empty;
        
        [Display(Name = "型號")]
        public string Model { get; set; } = string.Empty;
        
        [Display(Name = "貸款金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LoanAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "貸款期數(月)")]
        public int LoanPeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "下次繳款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime NextPaymentDate { get; set; }
    }

    // 綠能融資模型
    public class GreenEnergyFinance
    {
        public int Id { get; set; }
        
        [Display(Name = "客戶名稱")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "專案類型")]
        public string ProjectType { get; set; } = string.Empty;
        
        [Display(Name = "裝置容量")]
        public string Capacity { get; set; } = string.Empty;
        
        [Display(Name = "融資金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal LoanAmount { get; set; }
        
        [Display(Name = "月付金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyPayment { get; set; }
        
        [Display(Name = "融資期數(月)")]
        public int LoanPeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "下次繳款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime NextPaymentDate { get; set; }
        
        [Display(Name = "預估年減碳量(噸)")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal CarbonReduction { get; set; }
    }

    // 應收帳款受讓模型
    public class AccountsReceivableFinance
    {
        public int Id { get; set; }
        
        [Display(Name = "賣方公司")]
        public string SellerCompany { get; set; } = string.Empty;
        
        [Display(Name = "買方公司")]
        public string BuyerCompany { get; set; } = string.Empty;
        
        [Display(Name = "發票金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal InvoiceAmount { get; set; }
        
        [Display(Name = "預付比例(%)")]
        public int AdvanceRatio { get; set; }
        
        [Display(Name = "預付金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AdvanceAmount { get; set; }
        
        [Display(Name = "手續費率(%)")]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal FeeRate { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "申請日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "到期日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DueDate { get; set; }
        
        [Display(Name = "發票號碼")]
        public string InvoiceNumber { get; set; } = string.Empty;
    }

    // 不動產租賃模型
    public class RealEstateLease
    {
        public int Id { get; set; }
        
        [Display(Name = "承租人")]
        public string LesseeName { get; set; } = string.Empty;
        
        [Display(Name = "物件類型")]
        public string PropertyType { get; set; } = string.Empty;
        
        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;
        
        [Display(Name = "樓地板面積(坪)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal FloorArea { get; set; }
        
        [Display(Name = "月租金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MonthlyRent { get; set; }
        
        [Display(Name = "租期(月)")]
        public int LeasePeriod { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "租期開始日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime LeaseStartDate { get; set; }
        
        [Display(Name = "租期結束日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime LeaseEndDate { get; set; }
        
        [Display(Name = "押金")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Deposit { get; set; }
    }

    // 客戶管理模型
    public class FinanceCustomer
    {
        public int Id { get; set; }
        
        [Display(Name = "姓名")]
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "公司名稱")]
        public string CompanyName { get; set; } = string.Empty;
        
        [Display(Name = "身分證/統編")]
        public string IdNumber { get; set; } = string.Empty;
        
        [Display(Name = "電話")]
        public string Phone { get; set; } = string.Empty;
        
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "地址")]
        public string Address { get; set; } = string.Empty;
        
        [Display(Name = "信用評等")]
        public string CreditRating { get; set; } = string.Empty;
        
        [Display(Name = "總貸款金額")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalLoanAmount { get; set; }
        
        [Display(Name = "狀態")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "建檔日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreateDate { get; set; }
    }
}